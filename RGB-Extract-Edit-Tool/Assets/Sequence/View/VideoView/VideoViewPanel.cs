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

        RectTransform viewPanelRt;
        VideoViewPanelMenuPopup videoViewPanelMenuPopup;
        List<IPanelChannel> channels;

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

            //
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
            videoViewPanelMenuPopup.Show(false);

            if (eventData.button == PointerEventData.InputButton.Right)
            {
                if(videoViewPanelMenuPopup)
                {
                    videoViewPanelMenuPopup.SetPosition(TransformEx.GetRelativeAnchorPosition_Screen(viewPanelRt, eventData.position));
                    videoViewPanelMenuPopup.Show(true);
                }
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

        public void Apply(EditParam param)
        {
            channelSyncer.SyncAllPanel(this, param);
        }

        public void Sync(EditParam param)
        {
            if (param is CreateChannelParam create)
            {
                CreateChannel(true, create);
            }
            if (param is MoveChannelParam move)
            {
                MoveChannel(true, move);
            }
            if (param is DeleteChannelParam delete)
            {
                DelateChannel(true, delete);
            }
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

        #endregion
    }
}