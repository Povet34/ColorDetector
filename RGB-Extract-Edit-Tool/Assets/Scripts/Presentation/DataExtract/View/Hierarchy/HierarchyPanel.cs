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
        PanelSyncer panelSyncer;

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
                    onRenameGroup:          _RenameGroup,
                    onUngroupForFree:       _UnGroupForFree
                    ));
            hierarchyPanelMenuPopup.Show(false);
        }

        public void Init(DataExtractMain.PanelInjection injection)
        {
            channelUpdater = injection.channelUpdater;
            channelReceiver = injection.channelReceiver;
            panelSyncer = injection.panelSyncer;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            DisableMenuPopup(new DisableMenuPopupParam(this));

            if (eventData.button == PointerEventData.InputButton.Right)
            {
                // 오른쪽 클릭 처리
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
            // Raycast를 통해 클릭된 UI 요소 확인
            List<RaycastResult> results = new List<RaycastResult>();
            graphicRaycaster.Raycast(eventData, results);

            int groupIndex = -1; 
            List<int> channelIndices = null;

            foreach (RaycastResult result in results)
            {
                // 그룹인지 확인
                IPanelGroup selectedGroup = result.gameObject.GetComponent<IPanelGroup>();
                if (selectedGroup != null)
                {
                    groupIndex = selectedGroup.groupIndex;
                }
            }

            // selectChannels 확인
            if (selectChannels.Count > 0)
            {
                channelIndices = selectChannels.Select(ch => ch.channelIndex).ToList();
            }

            Vector2 localMousePosition = panelRt.InverseTransformPoint(eventData.position);

            // HierarchyPanelMenuPopup를 Show
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
                // 채널이 그룹에 속해 있지 않은 경우
                DeselectGroup(new DeselectGroupParam(this));
                DeselectChannel(new DeSelectChannelParam(this));
                SelectChannel(new SelectChannelParam(this, new List<int> { selectedChannel.channelIndex }));
            }
            else
            {
                // 채널이 그룹에 속해 있는 경우
                var parentGroup = selectedChannel.parentGroup;

                if (selectChannels.Count > 0 && selectChannels.All(ch => ch.parentGroup == parentGroup))
                {
                    // 이미 그룹이 선택된 상태에서 그룹 내 채널을 선택한 경우
                    DeselectGroup(new DeselectGroupParam(this));
                    DeselectChannel(new DeSelectChannelParam(this));
                    SelectChannel(new SelectChannelParam(this, new List<int> { selectedChannel.channelIndex }));
                }
                else
                {
                    // 그룹 선택 상태로 전환
                    DeselectGroup(new DeselectGroupParam(this));
                    DeselectChannel(new DeSelectChannelParam(this));
                    SelectGroup(new SelectGroupParam(this, parentGroup.groupIndex));
                }
            }
        }

        private void _HandleGroupSelection(IPanelGroup selectedGroup)
        {
            // 이전 선택 상태를 무조건 초기화
            DeselectChannel(new DeSelectChannelParam(this));
            DeselectGroup(new DeselectGroupParam(this));

            // 새로운 그룹 선택
            SelectGroup(new SelectGroupParam(this, selectedGroup.groupIndex));
        }

        void _SelectUIElement(PointerEventData eventData)
        {
            // Raycast를 통해 클릭된 UI 요소를 찾음  
            List<RaycastResult> results = new List<RaycastResult>();
            graphicRaycaster.Raycast(eventData, results);

            foreach (RaycastResult result in results)
            {
                // 채널 선택 처리
                IPanelChannel selectedChannel = result.gameObject.GetComponent<IPanelChannel>();
                if (selectedChannel != null)
                {
                    _HandleChannelSelection(selectedChannel);
                    return;
                }

                // 그룹 선택 처리
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

            // 그룹을 SiblingIndex 순서대로 정렬
            foreach (var group in groups.OrderBy(g => g.GetObject().transform.GetSiblingIndex()))
            {
                if (group == null)
                {
                    continue;
                }

                // 그룹의 SiblingIndex 설정
                group.GetObject().transform.SetSiblingIndex(siblingIndex++);

                // 그룹 내 채널을 inIndex 순서대로 정렬
                foreach (var channel in group.hasChannels.OrderBy(ch =>
                {
                    if (ch == null)
                    {
                        return int.MaxValue; // null 채널은 가장 뒤로 정렬
                    }
                    return ch.groupInIndex;
                }))
                {
                    if (channel != null)
                    {
                        // 채널의 SiblingIndex 설정
                        channel.GetObject().transform.SetSiblingIndex(siblingIndex++);
                    }
                    else
                    {
                        DLogger.LogError($"Null channel found in group {group.groupIndex} during sorting.");
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

        void _RenameGroup(int groupIndex)
        {
            InputFieldPopup.CreateAndShowPopup(GetComponentInParent<Canvas>().transform, "Rename Group", (newName) =>
            {
                RenameGroup(new RenameGroupParam(this, groupIndex, newName));
            });
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
            panelSyncer.SyncAllPanel(this, param);
        }

        public void Sync(EditParam param)
        {
            syncParamMap[param.editType].Invoke(param);
        }

        public void Refresh(RefreshParam param)
        {
            RefreshPanel(channelReceiver.GetChannels(), channelReceiver.GetGroups());
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

            RefreshPanel(channelReceiver.GetChannels(), channelReceiver.GetGroups());
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

            //채널들도 생성하고
            for (int i = 0; i < groupChannels.Count; i++)
            {
                groupChannels[i].SetGroup(gr, i);
            }

            groups.Add(gr);

            if (param.ownerPanel.Equals(this))
            {
                channelUpdater.MakeGroup(param);
                Apply(param);
            }

            RefreshPanel(channelReceiver.GetChannels(), channelReceiver.GetGroups());
        }

        public void MoveChannel(MoveChannelParam param)
        {
            if (param.ownerPanel.Equals(this))
            {
                channelUpdater.MoveChannel(param);
                Apply(param);
            }

            RefreshPanel(channelReceiver.GetChannels(), channelReceiver.GetGroups());
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
            // 기존의 채널과 그룹을 파괴
            _DestroyAll();

            // 채널리시버에서 최신 채널 정보를 가져와서 업데이트
            foreach (var updatedChannel in dataChannels)
            {
                if (updatedChannel == null)
                {
                    DLogger.LogError("Null channel found in dataChannels during RefreshPanel.");
                    continue;
                }

                if (hierarchyChannelPrefab == null || scrollViewContentRt == null)
                {
                    DLogger.LogError("HierarchyChannelPrefab or ScrollViewContentRt is null.");
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

            // 그룹도 업데이트
            foreach (var updatedGroup in dataGroups)
            {
                if (updatedGroup == null)
                {
                    DLogger.LogError("Null group found in dataGroups during RefreshPanel.");
                    continue;
                }

                if (hierarchyGroupPrefab == null || scrollViewContentRt == null)
                {
                    DLogger.LogError("HierarchyGroupPrefab or ScrollViewContentRt is null.");
                    continue;
                }

                IPanelGroup.Param createParam = new IPanelGroup.Param
                {
                    groupIndex = updatedGroup.groupIndex,
                    name = updatedGroup.name,
                    hasChannels = updatedGroup.hasChannels.Select(ch =>
                    {
                        if (ch == null)
                        {
                            Debug.LogError($"Null channel found in group {updatedGroup.groupIndex} during RefreshPanel.");
                            return null;
                        }
                        return channels.FirstOrDefault(c => c.channelIndex == ch.channelIndex);
                    }).ToList(),
                    sortDirection = updatedGroup.sortDirection,
                    onSort = _ChanageGroupSortDirection,
                };

                HierarchyGroup gr = Instantiate(hierarchyGroupPrefab, scrollViewContentRt);
                gr.Init(createParam);

                // 그룹에 속한 채널들에게 그룹 정보를 업데이트
                for (int i = 0; i < createParam.hasChannels.Count; i++)
                {
                    var channel = createParam.hasChannels[i];
                    if (channel != null)
                    {
                        channel.SetGroup(gr, i); // 그룹과 그룹 내 인덱스 설정
                    }
                    else
                    {
                        DLogger.LogError($"Null channel in group {updatedGroup.groupIndex} at index {i}.");
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
            if (param.ownerPanel.Equals(this))
            {
                channelUpdater.MoveDeltaGroup(param);
                Apply(param);
            }

            RefreshPanel(channelReceiver.GetChannels(), channelReceiver.GetGroups());
        }

        public void ChangeGroupSortDirection(ChangeGroupSortDirectionParam param)
        {
            // 그룹을 찾음
           var group = groups[param.groupIndex];
            if (group == null)
            {
                Debug.LogError($"Group with index {param.groupIndex} not found.");
                return;
            }

            // Apply 호출
            if (param.ownerPanel.Equals(this))
            {
                channelUpdater.ChangeGroupSortDirection(param);
                Apply(param);
            }

            // 채널리시버를 통해 그룹 정보를 가져옴
            var updatedGroup = channelReceiver.GetGroups().FirstOrDefault(g => g.groupIndex == param.groupIndex);
            if (updatedGroup == null)
            {
                DLogger.LogError($"Updated group with index {param.groupIndex} not found in ChannelReceiver.");
                return;
            }

            // 그룹 내 채널들의 inIndex를 업데이트
            for (int i = 0; i < updatedGroup.hasChannels.Count; i++)
            {
                var channel = channels.FirstOrDefault(ch => ch.channelIndex == updatedGroup.hasChannels[i].channelIndex);
                if (channel != null)
                {
                    channel.SetGroup(group, i); // 그룹과 새로운 inIndex 설정
                }
            }

            _SortPanel();
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
            // 1. 해당 그룹 찾기
            var group = groups.FirstOrDefault(gr => gr.groupIndex == param.groupIndex);
            if (group == null)
            {
                Debug.LogError($"Group with index {param.groupIndex} not found.");
                return;
            }

            // 2. 그룹 제거 
            groups.Remove(group);
            Destroy(group.GetObject());

            // 3. 그룹이 가지고 있는 채널들을 그룹 없음으로 업데이트
            foreach (var channel in group.hasChannels)
            {
                if (channel != null)
                {
                    channel.SetGroup(null, -1); // 그룹 정보 제거
                }
            }

            // 4. 상태 동기화
            if (param.ownerPanel.Equals(this))
            {
                channelUpdater.ReleaseGroup(param);
                Apply(param);
            }

            RefreshPanel(channelReceiver.GetChannels(), channelReceiver.GetGroups());
        }

        public void UnGroupForFree(UnGroupForFreeParam param)
        {
            if (param.ownerPanel.Equals(this))
            {
                channelUpdater.UnGroupForFree(param);
                Apply(param);
            }

            RefreshPanel(channelReceiver.GetChannels(), channelReceiver.GetGroups());
        }

        public void RenameGroup(RenameGroupParam param)
        {
            channelUpdater.RenameGroup(param);
            RefreshPanel(channelReceiver.GetChannels(), channelReceiver.GetGroups());
        }

        #endregion
    }
}