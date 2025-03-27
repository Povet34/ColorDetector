using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DataExtract
{

    public class HierarchyPanel : MonoBehaviour, IPanelSync, IPointerDownHandler
    {
        #region Injection

        ChannelUpdater channelUpdater;
        ChannelReceiver channelReceiver;
        ChannelSyncer channelSyncer;

        #endregion

        [SerializeField] HierarchyChannel hierarchyChannelPrefab;
        [SerializeField] RectTransform scrollViewContentRt;

        [SerializeField] GraphicRaycaster graphicRaycaster;
        [SerializeField] EventSystem eventSystem;

        List<IPanelChannel> channels;

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

            if (eventData.button == PointerEventData.InputButton.Left)
            {
                _SelectUIElement(eventData);
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

        public void CreateChannel(bool isSynced, CreateChannelParam param)
        {
            var ch = Instantiate(hierarchyChannelPrefab, scrollViewContentRt);
            ch.Init(param);
            channels.Add(ch);

            if (!isSynced)
            {
                channelUpdater.CreateChannel(param);
                Apply(param);
            }
        }

        public void MoveChannel(bool isSynced, MoveChannelParam param)
        {
            throw new NotImplementedException();
        }

        public void DelateChannel(bool isSynced, DeleteChannelParam param)
        {
            throw new NotImplementedException();
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