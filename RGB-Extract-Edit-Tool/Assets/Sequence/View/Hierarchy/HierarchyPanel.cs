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
        List<IPanelChannel> selectChannels;

        void Awake()
        {
            channels = new List<IPanelChannel>();
            selectChannels = new List<IPanelChannel>();
        }

        public void Init(ChannelUpdater channelUpdater, ChannelReceiver channelReceiver, ChannelSyncer channelSyncer)
        {
            this.channelUpdater = channelUpdater;
            this.channelReceiver = channelReceiver;
            this.channelSyncer = channelSyncer;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            DeselectChannel(new DeSelectChannelParam(this));

            if (eventData.button == PointerEventData.InputButton.Left)
            {
                _SelectUIElement(eventData);
            }
        }

        void DestroyAll()
        {
            foreach (var ch in channels)
            {
                Destroy(ch.GetObject());
            }

            channels.Clear();
            selectChannels.Clear();
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
                    SelectChannel(param);

                    return;
                }
            }
        }

        #region IPanelSync

        public Dictionary<eEditType, Action<EditParam>> syncParamMap => new Dictionary<eEditType, Action<EditParam>>()
        {
            { eEditType.CreateChannel, param => CreateChannel((CreateChannelParam)param) },
            { eEditType.DeleteChannel, param => DeleteChannel((DeleteChannelParam)param) },
            { eEditType.SelectChannel, param => SelectChannel((SelectChannelParam)param) },
            { eEditType.DeSelectChannel, param => DeselectChannel((DeSelectChannelParam)param) },
            { eEditType.MoveDeltaChannel, param => MoveDeltaChannel((MoveDeltaChannelParam)param) },
            { eEditType.Undo, param => Undo((UndoParam)param) },
            { eEditType.MakeGroup, param => MakeGroup((MakeGroupParam)param) },
        };


        public void Apply(EditParam param)
        {
            channelSyncer.SyncAllPanel(this, param);
        }

        public void Sync(EditParam param)
        {
            syncParamMap[param.editType].Invoke(param);
        }

        public void CreateChannel(CreateChannelParam param)
        {
            var ch = Instantiate(hierarchyChannelPrefab, scrollViewContentRt);
            ch.Init(param);
            channels.Add(ch);

            if (param.ownerPanel.Equals(this))
            {
                channelUpdater.CreateChannel(param);
                Apply(param);
            }
        }


        public void DeleteChannel(DeleteChannelParam param)
        {
            foreach (var delete in param.indices)
            {
                channels[delete].DestroyChannel();
            }

            if (param.ownerPanel.Equals(this))
            {
                channelUpdater.DeleteChannel(param);
                Apply(param);
            }
        }

        public void SelectChannel(SelectChannelParam param)
        {
            List<int> indices = param.indices;
            for (int i = 0; i < indices.Count; i++)
            {
                channels[indices[i]].Select();
            }

            if (param.ownerPanel.Equals(this))
            {
                Apply(param);
            }
        }

        public void DeselectChannel(DeSelectChannelParam param)
        {
            foreach (var ch in channels)
            {
                ch.Deselect();
            }

            if (param.ownerPanel.Equals(this))
            {
                Apply(param);
            }
        }

        public void MoveDeltaChannel(MoveDeltaChannelParam param)
        {
            foreach (int index in param.indices)
            {
                IPanelChannel channel = channels.Find(x => x.channelIndex == index);
                channel.MoveDelta(param.movePos);
            }

            if (param.ownerPanel.Equals(this))
            {
                channelUpdater.MoveDeltaChannel(param);
                Apply(param);
            }
        }

        public void Undo(UndoParam param)
        {
            if (null != param.state)
            {
                //다 없애고
                DestroyAll();

                //새로 재배치

                foreach (var newCh in param.state.channels)
                {
                    var ch = Instantiate(hierarchyChannelPrefab, scrollViewContentRt);
                    ch.Init(new CreateChannelParam(this, newCh.channelIndex, newCh.position));
                    channels.Add(ch);
                }
            }

            if (param.ownerPanel.Equals(this))
            {
                Apply(param);
            }
        }

        public void MakeGroup(MakeGroupParam param)
        {
            //그룹을 생성하고



            if (param.ownerPanel.Equals(this))
            {
                channelUpdater.MakeGroup(param);
                Apply(param);
            }
        }

        #endregion
    }
}