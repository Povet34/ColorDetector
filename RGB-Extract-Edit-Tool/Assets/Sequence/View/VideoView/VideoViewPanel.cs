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
            foreach (var ch in channels)
            {
                ch.Deselect();
            }

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

                    SelectChannelParm param = new SelectChannelParm()
                    {
                        indices = indices
                    };

                    SelectChannel(true, param);
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
            CreateChannelParam param = new CreateChannelParam
            {
                chIndex = channels.Count,
                createPos = position,
                ownerPanel = this,
                editType = eEditType.CreateChannel
            };

            CreateChannel(false, param);
            Apply(param);
        }

        #region IPanelSync

        public Dictionary<eEditType, Action<EditParam>> syncParamMap => new Dictionary<eEditType, Action<EditParam>>()
        {
            { eEditType.CreateChannel, param => CreateChannel(true, (CreateChannelParam)param) },
            { eEditType.MoveChannel, param => MoveChannel(true, (MoveChannelParam)param) },
            { eEditType.DeleteChannel, param => DelateChannel(true, (DeleteChannelParam)param) },
            { eEditType.SelectChannel, param => SelectChannel(true, (SelectChannelParm)param) }
        };

        public void Apply(EditParam param)
        {
            channelSyncer.SyncAllPanel(this, param);
        }

        public void Sync(EditParam param)
        {
            syncParamMap[param.editType].Invoke(param);
        }

        public void MoveChannel(bool isSync, MoveChannelParam param)
        {
        }

        public void DelateChannel(bool isSync, DeleteChannelParam param)
        {
        }

        public void CreateChannel(bool isSync, CreateChannelParam param)
        {
            var ch = Instantiate(videoViewChannelPrefab, transform);
            ch.Init(param);
            channels.Add(ch);

            if (!isSync)
                channelUpdater.CreateChannel(param);
        }

        public void SelectChannel(bool isSync, SelectChannelParm param)
        {
            List<int> indices = param.indices;
            for (int i = 0; i < indices.Count; i++)
            {
                channels[indices[i]].Select();
            }

            if (!isSync)
                channelUpdater.SelectChannel(param);
        }

        #endregion
    }
}