using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace DataExtract
{

    public class HierarchyPanel : MonoBehaviour, IPanelSync
    {
        #region Injection

        ChannelUpdater channelUpdater;
        ChannelReceiver channelReceiver;
        ChannelSyncer channelSyncer;

        #endregion

        [SerializeField] HierarchyChannel hierarchyChannelPrefab;
        [SerializeField] RectTransform scrollViewContentRt;

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
            foreach (var ch in channels)
            {
                ch.Deselect();
            }

            if (eventData.button == PointerEventData.InputButton.Left)
            {
                CreateChannelParam param = new CreateChannelParam
                {
                    chIndex = channels.Count,
                    createPos = Vector2.zero,
                    ownerPanel = this,
                    editType = eEditType.CreateChannel
                };

                CreateChannel(false, param);
                Apply(param);
            }
            else if (eventData.button == PointerEventData.InputButton.Right)
            {
                // 오른쪽 버튼 클릭 시의 동작을 여기에 추가
                DLogger.Log("Right mouse button clicked");
            }
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

        public void CreateChannel(bool isSync, CreateChannelParam param)
        {
            var ch = Instantiate(hierarchyChannelPrefab, scrollViewContentRt);
            ch.Init(param);
            channels.Add(ch);

            if (!isSync)
                channelUpdater.CreateChannel(param);
        }

        public void MoveChannel(bool isSync, MoveChannelParam param)
        {
            throw new NotImplementedException();
        }

        public void DelateChannel(bool isSync, DeleteChannelParam param)
        {
            throw new NotImplementedException();
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