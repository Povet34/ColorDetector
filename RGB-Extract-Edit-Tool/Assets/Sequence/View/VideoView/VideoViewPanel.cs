using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;


namespace DataExtract
{

    public class VideoViewPanel : MonoBehaviour, IPanelSync, IPointerDownHandler
    {
        #region Injection

        ChannelUpdater channelUpdater;
        ChannelReceiver channelReceiver;
        ChannelSyncer channelSyncer;

        #endregion

        [SerializeField] VideoViewChannel videoViewChannelPrefab;
        [SerializeField] VideoViewPanelMenuPopup videoViewPanelMenuPopupPrefab;

        [SerializeField] GraphicRaycaster graphicRaycaster;
        [SerializeField] EventSystem eventSystem;

        List<IPanelChannel> channels;
        RectTransform viewPanelRt;
        VideoViewPanelMenuPopup videoViewPanelMenuPopup;

        void Awake()
        {
            viewPanelRt = GetComponent<RectTransform>();

            //MeunPopup
            videoViewPanelMenuPopup = Instantiate(videoViewPanelMenuPopupPrefab, transform);
            videoViewPanelMenuPopup.Init(
                new VideoViewPanelMenuPopup.MenuActions(
                        onCreateChannel:    _CreateChannel,
                        onCreateSegment:    null,
                        onDeleteChannel:    null,
                        onMakeGroup:        null,
                        onReleaseGroup:     null
                    ));
            videoViewPanelMenuPopup.Show(false);

            //UI Select
        }

        public void Init(ChannelUpdater channelUpdater, ChannelReceiver channelReceiver, ChannelSyncer channelSyncer)
        {
            this.channelUpdater = channelUpdater;
            this.channelReceiver = channelReceiver;
            this.channelSyncer = channelSyncer;

            channels = new List<IPanelChannel>();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            DeSelectChannel(false, new DeSelectChannelParam(this));

            videoViewPanelMenuPopup.Show(false);
            Vector2 viewerPos = TransformEx.GetRelativeAnchorPosition_Screen(viewPanelRt, eventData.position);

            if (eventData.button == PointerEventData.InputButton.Left)
            {
                _SelectUIElement(eventData);
            }
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                _ShowVideoViewPanelMenuPopup(viewerPos);
            }
        }


        void _SelectUIElement(PointerEventData eventData)
        {
            List<RaycastResult> results = new List<RaycastResult>();
            graphicRaycaster.Raycast(eventData, results);

            foreach (RaycastResult result in results)
            {
                IPanelChannel selectedChannel = result.gameObject.GetComponent<IPanelChannel>();
                if (selectedChannel != null)
                {
                    List<int> indices = new List<int>();
                    indices.Add(selectedChannel.channelIndex);

                    SelectChannelParam param = new SelectChannelParam(this, indices);
                    SelectChannel(false, param);

                    return;
                }
            }
        }

        void _ShowVideoViewPanelMenuPopup(Vector2 pos)
        {
            if (videoViewPanelMenuPopup)
            {
                videoViewPanelMenuPopup.SetPosition(pos);
                videoViewPanelMenuPopup.Show(true);
            }
        }

        void _CreateChannel(Vector2 position)
        {
            CreateChannelParam param = new CreateChannelParam(this, channels.Count, position);
            CreateChannel(false, param);
        }

        #region IPanelSync

        public Dictionary<eEditType, Action<EditParam>> syncParamMap => new Dictionary<eEditType, Action<EditParam>>()
        {
            { eEditType.CreateChannel, param => CreateChannel(true, (CreateChannelParam)param) },
            { eEditType.MoveChannel, param => MoveChannel(true, (MoveChannelParam)param) },
            { eEditType.DeleteChannel, param => DelateChannel(true, (DeleteChannelParam)param) },
            { eEditType.SelectChannel, param => SelectChannel(true, (SelectChannelParam)param) },
            { eEditType.DeSelectChannel, param => DeSelectChannel(true, (DeSelectChannelParam)param) },
        };

        public void Apply(EditParam param)
        {
            channelSyncer.SyncAllPanel(this, param);
        }

        public void Sync(EditParam param)
        {
            syncParamMap[param.editType].Invoke(param);
        }

        public void MoveChannel(bool isSynced, MoveChannelParam param)
        {
        }

        public void DelateChannel(bool isSynced, DeleteChannelParam param)
        {
        }

        public void CreateChannel(bool isSynced, CreateChannelParam param)
        {
            var ch = Instantiate(videoViewChannelPrefab, transform);
            ch.Init(param);
            channels.Add(ch);

            if (!isSynced)
            {
                channelUpdater.CreateChannel(param);
                Apply(param);
            }
        }

        public void SelectChannel(bool isSynced, SelectChannelParam param)
        {
            List<int> indices = param.indices;
            for (int i = 0; i < indices.Count; i++)
            {
                channels[indices[i]].Select();
            }

            if (!isSynced)
            {
                channelUpdater.SelectChannel(param);
                Apply(param);
            }
        }

        public void DeSelectChannel(bool isSynced, DeSelectChannelParam param)
        {
            foreach (var ch in channels)
            {
                ch.Deselect();
            }

            if (!isSynced)
            {
                channelUpdater.DeSelectChannel(param);
                Apply(param);
            }
        }

        #endregion
    }
}