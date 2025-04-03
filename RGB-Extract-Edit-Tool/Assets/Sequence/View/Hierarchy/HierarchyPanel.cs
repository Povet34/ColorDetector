using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEditor;
using System.Linq;

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
        [SerializeField] HierarchyGroup hierarchyGroupPrefab;

        [SerializeField] RectTransform scrollViewContentRt;

        [SerializeField] GraphicRaycaster graphicRaycaster;
        [SerializeField] EventSystem eventSystem;

        List<IPanelChannel> channels;
        List<IPanelChannel> selectChannels;
        List<IPanelGroup> groups;

        void Awake()
        {
            channels = new List<IPanelChannel>();
            selectChannels = new List<IPanelChannel>();
            groups = new List<IPanelGroup>();
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

            foreach(var gr in groups)
            {
                Destroy(gr.GetObject());
            }

            channels.Clear();
            selectChannels.Clear();
            groups.Clear();
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

        void _MoveChannel(int index, Vector2 movePos)
        {
            MoveChannelParam param = new MoveChannelParam(this, new List<int>() { index }, movePos);
            MoveChannel(param);
        }

        void _SortPanel()
        {
            int siblingIndex = 0;
            foreach (var group in groups.OrderBy(g => g.GetObject().transform.GetSiblingIndex()))
            {
                group.GetObject().transform.SetSiblingIndex(siblingIndex++);

                foreach (var channelIndex in group.channelIndices)
                {
                    var channel = channels.FirstOrDefault(ch => ch.channelIndex == channelIndex);
                    if (channel != null)
                    {
                        channel.GetObject().transform.SetSiblingIndex(siblingIndex++);
                    }
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
            { eEditType.MoveChannel, param => MoveChannel((MoveChannelParam)param) },
            { eEditType.SelectGroup, param => SelectGroup((SelectGroupParam)param) },
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
            HierarchyChannel ch = Instantiate(hierarchyChannelPrefab, scrollViewContentRt);

            HierarchyChannel.CreateParam createParam = new HierarchyChannel.CreateParam();
            createParam.createPos = param.createPos;
            createParam.chIndex = param.chIndex;
            createParam.onMoveCallback = _MoveChannel;

            ch.Init(createParam);
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
                var selected = channels[indices[i]];

                selected.Select();
                selectChannels.Add(selected);
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

            selectChannels.Clear();

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
                    var createParam = new HierarchyChannel.CreateParam();
                    createParam.chIndex = newCh.channelIndex;
                    createParam.createPos = newCh.position;

                    HierarchyChannel ch = Instantiate(hierarchyChannelPrefab, scrollViewContentRt);
                    ch.Init(createParam);
                    channels.Add(ch);
                }

                foreach(var newGr in param.state.groups)
                {
                    var createParam = new MakeGroupParam(this, newGr.groupIndex, newGr.hasChannels, IGroup.SortDirection.Left, newGr.name);

                    var gr = Instantiate(hierarchyGroupPrefab, scrollViewContentRt);
                    gr.Init(createParam);

                    for (int i = 0; i < newGr.hasChannels.Count; i++)
                    {
                        channels[newGr.hasChannels[i]].SetGroup(gr, i);
                    }

                    groups.Add(gr);
                }

                _SortPanel();
            }

            if (param.ownerPanel.Equals(this))
            {
                Apply(param);
            }
        }

        public void MakeGroup(MakeGroupParam param)
        {
            //그룹을 생성하고

            HierarchyGroup gr = Instantiate(hierarchyGroupPrefab, scrollViewContentRt);
            gr.Init(param);

            //채널들도 생성하고
            for (int i = 0; i < selectChannels.Count; i++)
            {
                selectChannels[i].SetGroup(gr, i);
            }

            groups.Add(gr);

            _SortPanel();

            if (param.ownerPanel.Equals(this))
            {
                channelUpdater.MakeGroup(param);
                Apply(param);
            }
        }

        public void MoveChannel(MoveChannelParam param)
        {
            foreach(var index in param.indices)
            {
                channels[index].position = param.position;
            }

            if (param.ownerPanel.Equals(this))
            {
                channelUpdater.MoveChannel(param);
                Apply(param);
            }
        }

        public void SelectGroup(SelectGroupParam param)
        {
            foreach (var index in groups[param.groupIndex].channelIndices)
            {
                channels[index].Select();
                selectChannels.Add(channels[index]);
            }

            if (param.ownerPanel.Equals(this))
            {
                Apply(param);
            }
        }

        #endregion
    }
}