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
    public class HierarchyPanel : MonoBehaviour, IPanelSync, IPointerDownHandler, IPointerUpHandler
    {
        #region Injection

        ChannelUpdater channelUpdater;
        ChannelReceiver channelReceiver;
        ChannelSyncer channelSyncer;

        #endregion

        [SerializeField] HierarchyPanelMenuPopup hierarchyPanelMenuPopupPrefab;
        [SerializeField] HierarchyChannel hierarchyChannelPrefab;
        [SerializeField] HierarchyGroup hierarchyGroupPrefab;

        [SerializeField] RectTransform scrollViewContentRt;

        [SerializeField] GraphicRaycaster graphicRaycaster;
        [SerializeField] EventSystem eventSystem;

        RectTransform panelRt;

        HierarchyPanelMenuPopup hierarchyPanelMenuPopup;
        List<IPanelChannel> channels;
        List<IPanelChannel> selectChannels;
        List<IPanelGroup> groups;


        void Awake()
        {
            channels = new List<IPanelChannel>();
            selectChannels = new List<IPanelChannel>();
            groups = new List<IPanelGroup>();

            hierarchyPanelMenuPopup = Instantiate(hierarchyPanelMenuPopupPrefab, transform);

            //GetComp
            panelRt = GetComponent<RectTransform>();

            hierarchyPanelMenuPopup.Init(
                new HierarchyPanelMenuPopup.MenuActions(
                    onAddChannelsToGroup:   null,
                    onDeleteChannel:        _DeleteChannel,
                    onMakeGroup:            _MakeGroup,
                    onReleaseGroup:         _ReleaseGroup,
                    onRenameGroup:          null,
                    onUngroupForFree:       null
                    ));
            hierarchyPanelMenuPopup.Show(false);
        }

        public void Init(ChannelUpdater channelUpdater, ChannelReceiver channelReceiver, ChannelSyncer channelSyncer)
        {
            this.channelUpdater = channelUpdater;
            this.channelReceiver = channelReceiver;
            this.channelSyncer = channelSyncer;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            DisableMenuPopup(new DisableMenuPopupParam(this));

            if (eventData.button == PointerEventData.InputButton.Right)
            {
                // ������ Ŭ�� ó��
                _HandleRightClick(eventData);
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                _SelectUIElement(eventData);
            }
        }


        private void _HandleRightClick(PointerEventData eventData)
        {
            // Raycast�� ���� Ŭ���� UI ��� Ȯ��
            List<RaycastResult> results = new List<RaycastResult>();
            graphicRaycaster.Raycast(eventData, results);

            int groupIndex = -1; 
            List<int> channelIndices = null;

            foreach (RaycastResult result in results)
            {
                // �׷����� Ȯ��
                IPanelGroup selectedGroup = result.gameObject.GetComponent<IPanelGroup>();
                if (selectedGroup != null)
                {
                    groupIndex = selectedGroup.groupIndex;
                }
            }

            // selectChannels Ȯ��
            if (selectChannels.Count > 0)
            {
                channelIndices = selectChannels.Select(ch => ch.channelIndex).ToList();
            }

            Vector2 localMousePosition = panelRt.InverseTransformPoint(eventData.position);

            // HierarchyPanelMenuPopup�� Show
            hierarchyPanelMenuPopup.SetPosition(localMousePosition);
            hierarchyPanelMenuPopup.Show(true, groupIndex, channelIndices);
        }


        void _DestroyAll()
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

        private void _HandleChannelSelection(IPanelChannel selectedChannel)
        {
            if (!selectedChannel.HasGroup())
            {
                // ä���� �׷쿡 ���� ���� ���� ���
                DeselectGroup(new DeselectGroupParam(this));
                DeselectChannel(new DeSelectChannelParam(this));
                SelectChannel(new SelectChannelParam(this, new List<int> { selectedChannel.channelIndex }));
            }
            else
            {
                // ä���� �׷쿡 ���� �ִ� ���
                var parentGroup = selectedChannel.parentGroup;

                if (selectChannels.Count > 0 && selectChannels.All(ch => ch.parentGroup == parentGroup))
                {
                    // �̹� �׷��� ���õ� ���¿��� �׷� �� ä���� ������ ���
                    DeselectGroup(new DeselectGroupParam(this));
                    DeselectChannel(new DeSelectChannelParam(this));
                    SelectChannel(new SelectChannelParam(this, new List<int> { selectedChannel.channelIndex }));
                }
                else
                {
                    // �׷� ���� ���·� ��ȯ
                    DeselectGroup(new DeselectGroupParam(this));
                    DeselectChannel(new DeSelectChannelParam(this));
                    SelectGroup(new SelectGroupParam(this, parentGroup.groupIndex));
                }
            }
        }

        private void _HandleGroupSelection(IPanelGroup selectedGroup)
        {
            // ���� ���� ���¸� ������ �ʱ�ȭ
            DeselectChannel(new DeSelectChannelParam(this));
            DeselectGroup(new DeselectGroupParam(this));

            // ���ο� �׷� ����
            SelectGroup(new SelectGroupParam(this, selectedGroup.groupIndex));
        }

        void _SelectUIElement(PointerEventData eventData)
        {
            // Raycast�� ���� Ŭ���� UI ��Ҹ� ã��  
            List<RaycastResult> results = new List<RaycastResult>();
            graphicRaycaster.Raycast(eventData, results);

            foreach (RaycastResult result in results)
            {
                // ä�� ���� ó��
                IPanelChannel selectedChannel = result.gameObject.GetComponent<IPanelChannel>();
                if (selectedChannel != null)
                {
                    _HandleChannelSelection(selectedChannel);
                    return;
                }

                // �׷� ���� ó��
                IPanelGroup selectedGroup = result.gameObject.GetComponent<IPanelGroup>();
                if (selectedGroup != null)
                {
                    _HandleGroupSelection(selectedGroup);
                    return;
                }
            }
        }

        void _MoveChannel(int index, Vector2 movePos)
        {
            MoveChannelParam param = new MoveChannelParam(this, new List<int>() { index }, movePos);
            MoveChannel(param);
        }

        void _MakeGroup()
        {
            int groupIndex = channelReceiver.GetGroupCount();

            List<int> indices = new List<int>();
            foreach (var ch in selectChannels)
            {
                indices.Add(ch.channelIndex);
            }

            if (channelReceiver.CanGroup(indices))
            {
                MakeGroupParam param = new MakeGroupParam(this, groupIndex, indices, IGroup.SortDirection.Left, Definitions.GetDefaultGroupName(groupIndex.ToString()));

                MakeGroup(param);
            }
        }

        void _SortPanel()
        {
            int siblingIndex = 0;

            // �׷��� SiblingIndex ������� ����
            foreach (var group in groups.OrderBy(g => g.GetObject().transform.GetSiblingIndex()))
            {
                // �׷��� SiblingIndex ����
                group.GetObject().transform.SetSiblingIndex(siblingIndex++);

                // �׷� �� ä���� inIndex ������� ����
                foreach (var channel in group.hasChannels.OrderBy(ch => ch.groupInIndex))
                {
                    if (channel != null)
                    {
                        // ä���� SiblingIndex ����
                        channel.GetObject().transform.SetSiblingIndex(siblingIndex++);
                    }
                }
            }
        }

        void _ChanageGroupSortDirection(int groupIndex, IGroup.SortDirection direction)
        {
            var param = new ChangeGroupSortDirectionParam(this, groupIndex, direction);
            ChangeGroupSortDirection(param);
        }

        void _DeleteChannel()
        {
            List<int> indices = selectChannels.Select(ch => ch.channelIndex).ToList();

            DeselectChannel(new DeSelectChannelParam(this));
            DeleteChannel(new DeleteChannelParam(this, indices));
        }

        void _ReleaseGroup(int groupInex)
        {
            ReleaseGroup(new ReleaseGroupParam(this, groupInex));
        }

        void _UnGroupForFree()
        {
            List<int> indices = selectChannels.Select(ch => ch.channelIndex).ToList();
            UnGroupForFree(new UnGroupForFreeParam(this, indices));
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
            { eEditType.DeselectGroup, param => DeselectGroup((DeselectGroupParam)param) },
            { eEditType.MoveDeltaGroup, param => MoveDeltaGroup((MoveDeltaGroupParam)param) },
            { eEditType.ChangeGroupSortDirection, param => ChangeGroupSortDirection((ChangeGroupSortDirectionParam)param) },
            { eEditType.DisableMenuPopup, param => DisableMenuPopup((DisableMenuPopupParam)param) },
            { eEditType.ReleaseGroup, param => ReleaseGroup((ReleaseGroupParam)param) },
            { eEditType.UnGroupForFree, param => UnGroupForFree((UnGroupForFreeParam)param) }
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

            RefreshPanel(channelReceiver.GetChannels(), channelReceiver.GetGroups());
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
                RefreshPanel(param.state.channels, param.state.groups);
            }

            if (param.ownerPanel.Equals(this))
            {
                Apply(param);
            }
        }

        public void MakeGroup(MakeGroupParam param)
        {
            HierarchyGroup gr = Instantiate(hierarchyGroupPrefab, scrollViewContentRt);

            List<IPanelChannel> groupChannels = param.channelIndices.Select(index => channels.FirstOrDefault(ch => ch.channelIndex == index)).ToList();

            IPanelGroup.Param createParam = new IPanelGroup.Param();
            createParam.groupIndex = param.groupIndex;
            createParam.name = param.name;
            createParam.hasChannels = groupChannels;
            createParam.sortDirection = param.sortDirection;
            createParam.onSort = _ChanageGroupSortDirection;

            gr.Init(createParam);
            gr.Select();

            //ä�ε鵵 �����ϰ�
            for (int i = 0; i < groupChannels.Count; i++)
            {
                groupChannels[i].SetGroup(gr, i);
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
                channels[index].position = param.movePos;
            }

            if (param.ownerPanel.Equals(this))
            {
                channelUpdater.MoveChannel(param);
                Apply(param);
            }
        }

        public void SelectGroup(SelectGroupParam param)
        {
            var group = groups[param.groupIndex];
            foreach (var channel in group.hasChannels)
            {
                channel.Select();
                selectChannels.Add(channel);
            }

            group.Select();

            if (param.ownerPanel.Equals(this))
            {
                Apply(param);
            }
        }

        public void RefreshPanel(List<IChannel> dataChannels, List<IGroup> dataGroups)
        {
            // ������ ä�ΰ� �׷��� �ı�
            _DestroyAll();

            // ä�θ��ù����� �ֽ� ä�� ������ �����ͼ� ������Ʈ
            foreach (var updatedChannel in dataChannels)
            {
                if (hierarchyChannelPrefab == null || scrollViewContentRt == null)
                {
                    Debug.LogError("HierarchyChannelPrefab or ScrollViewContentRt is null.");
                    continue;
                }

                var createParam = new HierarchyChannel.CreateParam
                {
                    chIndex = updatedChannel.channelIndex,
                    createPos = updatedChannel.position,
                    onMoveCallback = _MoveChannel
                };

                HierarchyChannel ch = Instantiate(hierarchyChannelPrefab, scrollViewContentRt);
                ch.Init(createParam);
                channels.Add(ch);
            }

            // �׷쵵 ������Ʈ
            foreach (var updatedGroup in dataGroups)
            {
                if (hierarchyGroupPrefab == null || scrollViewContentRt == null)
                {
                    Debug.LogError("HierarchyGroupPrefab or ScrollViewContentRt is null.");
                    continue;
                }

                IPanelGroup.Param createParam = new IPanelGroup.Param
                {
                    groupIndex = updatedGroup.groupIndex,
                    name = updatedGroup.name,
                    hasChannels = updatedGroup.hasChannels.Select(ch => channels.FirstOrDefault(c => c.channelIndex == ch.channelIndex)).ToList()
                };

                HierarchyGroup gr = Instantiate(hierarchyGroupPrefab, scrollViewContentRt);
                gr.Init(createParam);

                // �׷쿡 ���� ä�ε鿡�� �׷� ������ ������Ʈ
                for (int i = 0; i < createParam.hasChannels.Count; i++)
                {
                    var channel = createParam.hasChannels[i];
                    if (channel != null)
                    {
                        channel.SetGroup(gr, i); // �׷�� �׷� �� �ε��� ����
                    }
                }

                groups.Add(gr);
            }

            _SortPanel();
        }

        public void DeselectGroup(DeselectGroupParam param)
        {
            foreach (var gr in groups)
            {
                gr.Deselect();
            }

            foreach (var ch in channels)
            {
                ch.Deselect();
            }


            if (param.ownerPanel.Equals(this))
            {
                Apply(param);
            }
        }

        public void MoveDeltaGroup(MoveDeltaGroupParam param)
        {
            foreach (var index in param.indices)
            {
                channels[index].MoveDelta(param.movePos);
            }

            if (param.ownerPanel.Equals(this))
            {
                channelUpdater.MoveDeltaGroup(param);
                Apply(param);
            }
        }

        public void ChangeGroupSortDirection(ChangeGroupSortDirectionParam param)
        {
            // �׷��� ã��
            var group = groups[param.groupIndex];
            if (group == null)
            {
                Debug.LogError($"Group with index {param.groupIndex} not found.");
                return;
            }

            // Apply ȣ��
            if (param.ownerPanel.Equals(this))
            {
                channelUpdater.ChangeGroupSortDirection(param);
                Apply(param);
            }

            // ä�θ��ù��� ���� �׷� ������ ������
            var updatedGroup = channelReceiver.GetGroups().FirstOrDefault(g => g.groupIndex == param.groupIndex);
            if (updatedGroup == null)
            {
                DLogger.LogError($"Updated group with index {param.groupIndex} not found in ChannelReceiver.");
                return;
            }

            // �׷� �� ä�ε��� inIndex�� ������Ʈ
            for (int i = 0; i < updatedGroup.hasChannels.Count; i++)
            {
                var channel = channels.FirstOrDefault(ch => ch.channelIndex == updatedGroup.hasChannels[i].channelIndex);
                if (channel != null)
                {
                    channel.SetGroup(group, i); // �׷�� ���ο� inIndex ����
                }
            }

            _SortPanel();

            DLogger.Log($"Group {param.groupIndex} channels' inIndex updated.");
        }

        public void DisableMenuPopup(DisableMenuPopupParam param)
        {
            hierarchyPanelMenuPopup.Show(false);
            //Apply
            if (param.ownerPanel.Equals(this))
            {
                Apply(param);
            }
        }

        public void ReleaseGroup(ReleaseGroupParam param)
        {
            // 1. �ش� �׷� ã��
            var group = groups.FirstOrDefault(gr => gr.groupIndex == param.groupIndex);
            if (group == null)
            {
                Debug.LogError($"Group with index {param.groupIndex} not found.");
                return;
            }

            // 2. �׷� ���� 
            groups.Remove(group);
            Destroy(group.GetObject());

            // 3. �׷��� ������ �ִ� ä�ε��� �׷� �������� ������Ʈ
            foreach (var channel in group.hasChannels)
            {
                if (channel != null)
                {
                    channel.SetGroup(null, -1); // �׷� ���� ����
                }
            }

            // 4. ���� ����ȭ
            if (param.ownerPanel.Equals(this))
            {
                channelUpdater.ReleaseGroup(param);
                Apply(param);
            }

            // 5. �г� ���ΰ�ħ
            var updatedChannels = channelReceiver.GetChannels();
            var updatedGroups = channelReceiver.GetGroups();
            RefreshPanel(updatedChannels, updatedGroups);
        }

        public void UnGroupForFree(UnGroupForFreeParam param)
        {
            if (param.ownerPanel.Equals(this))
            {
                channelUpdater.UnGroupForFree(param);
                Apply(param);
            }
        }

        #endregion
    }
}