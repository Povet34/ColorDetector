using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;


namespace DataExtract
{

    public class VideoViewPanel : MonoBehaviour,
        IPanelSync,
        IPointerDownHandler, 
        IPointerUpHandler
    {
        #region Injection

        ChannelUpdater channelUpdater;
        ChannelReceiver channelReceiver;
        ChannelSyncer channelSyncer;
        VideoDataReceiver videoDataReceiver;

        #endregion

        [SerializeField] VideoViewChannel videoViewChannelPrefab;
        [SerializeField] VideoViewGroup videoViewGroupPrefab;
        [SerializeField] VideoViewPanelMenuPopup videoViewPanelMenuPopupPrefab;

        [SerializeField] GraphicRaycaster graphicRaycaster;
        [SerializeField] EventSystem eventSystem;

        List<IPanelChannel> channels;
        List<IPanelChannel> selectChannels;
        List<IPanelGroup> groups;

        RectTransform panelRt;

        RectAreaChannelSelection rectAreaChannelSelection;
        VideoViewPanelMenuPopup videoViewPanelMenuPopup;
        RawImage videoViewImage;

        void Awake()
        {
            //Channel Init
            channels = new List<IPanelChannel>();
            selectChannels = new List<IPanelChannel>();
            groups = new List<IPanelGroup>();

            //GetComp
            panelRt = GetComponent<RectTransform>();
            videoViewImage = GetComponent<RawImage>();

            //MeunPopup
            videoViewPanelMenuPopup = Instantiate(videoViewPanelMenuPopupPrefab, transform);
            videoViewPanelMenuPopup.Init(
                new VideoViewPanelMenuPopup.MenuActions(
                        onCreateChannel:    _CreateChannel,
                        onCreateSegment:    null,
                        onDeleteChannel:    _DeleteChannel,
                        onMakeGroup:        _MakeGroup,
                        onReleaseGroup:     _ReleaseGroup,
                        onUnGroupForFree:   _UnGroupForFree
                    ));
            videoViewPanelMenuPopup.Show(false);

            //UI Rect Select
            rectAreaChannelSelection = RectAreaChannelSelection.Create(
                panelRt,
                this,
                ref channels, 
                SelectChannel, 
                DeselectChannel
                );
        }

        public void Init(DataExtractMain.PanelInjection injection)
        {
            channelUpdater = injection.channelUpdater;
            channelReceiver = injection.channelReceiver;
            channelSyncer = injection.channelSyncer;
            videoDataReceiver = injection.videoDataReceiver;

            videoDataReceiver.RegistUpdateVideoTexture(ChnageVideoViewImage);
        }

        void OnDestroy()
        {
            videoDataReceiver.UnregistUpdateVideoTexture(ChnageVideoViewImage);
        }

        void Update()
        {
            bool isKeyDelete = Input.GetKeyDown(KeyCode.Delete);

            if (isKeyDelete)
            {
                _DeleteChannel();
            }

            bool isLeftCtrl = Input.GetKey(KeyCode.LeftControl);
            bool isKeyZ = Input.GetKeyDown(KeyCode.Z);

            if (isLeftCtrl && isKeyZ)
            {
                var pr = channelUpdater.Undo();
                Undo(new UndoParam(this, pr));
            }
        }

        void DestroyAll()
        {
            foreach (var ch in channels)
            {
                Destroy(ch.GetObject());
            }

            foreach (var gr in groups)
            {
                Destroy(gr.GetObject());
            }

            channels.Clear();
            selectChannels.Clear();
            groups.Clear();
        }

        private void ChnageVideoViewImage()
        {
            videoViewImage.texture = videoDataReceiver.GetVideoTexture();
        }


        #region UI Select

        public void OnPointerDown(PointerEventData eventData)
        {
            DisableMenuPopup(new DisableMenuPopupParam(this));

            if (eventData.button == PointerEventData.InputButton.Left)
            {
                _HandleLeftClick(eventData);
            }
            else if (eventData.button == PointerEventData.InputButton.Right)
            {
                _HandleRightClick(eventData);
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                Vector2 localMousePosition = panelRt.InverseTransformPoint(eventData.position);
                if (_moveState.IsMoving(localMousePosition) && (_moveState.IsGroupSelected() || _moveState.IsChannelsSelected()))
                {
                    if (_moveState.IsGroupSelected())
                    {
                        _MoveDeltaGroup(_moveState.GetSelectedGroup(), localMousePosition);
                    }
                    else if (_moveState.IsChannelsSelected())
                    {
                        _MoveDeltaChannels(localMousePosition);
                    }

                    _moveState.EndMove();
                }
                else
                {
                    IPanelChannel clickedChannel = GetChannelAtPosition(eventData);
                    if (clickedChannel != null)
                    {
                        if (rectAreaChannelSelection.IsRectActive())
                        {
                            _SelectRect();
                            return;
                        }

                        if (clickedChannel.HasGroup())
                        {
                            DeselectChannel(new DeSelectChannelParam(this));
                            DeselectGroup(new DeselectGroupParam(this));

                            //같은 그룹 내에 있는 채널을 또 선택하면, 강제 채널 하나만 선택되도록한다.
                            if (_moveState.IsSameGroup(clickedChannel.parentGroup))
                            {
                                SelectChannel(new SelectChannelParam(this, new List<int> { clickedChannel.channelIndex }));
                                _moveState.Select(null, new List<IPanelChannel>() { clickedChannel });

                                return;
                            }

                            SelectGroup(new SelectGroupParam(this, clickedChannel.parentGroup.groupIndex));
                            _moveState.Select(clickedChannel.parentGroup, new List<IPanelChannel> { clickedChannel });
                        }
                        else
                        {
                            SelectChannel(new SelectChannelParam(this, new List<int> { clickedChannel.channelIndex }));
                            _moveState.Select(null, new List<IPanelChannel>() { clickedChannel });
                        }
                    }
                    else
                    {
                        _SelectRect();
                    }
                }
            }


            void _SelectRect()
            {
                _moveState.Deselect();
                var indices = rectAreaChannelSelection.EndFindSelect();
                List<IPanelChannel> selectedChannels = new List<IPanelChannel>();
                foreach (int index in indices)
                {
                    IPanelChannel channel = channels.FirstOrDefault(ch => ch.channelIndex == index);
                    if (channel != null)
                    {
                        selectedChannels.Add(channel);
                    }
                }
                if (selectedChannels.Count > 0)
                {
                    _moveState.Select(null, selectedChannels);
                }
            }
        }

        private IPanelChannel GetChannelAtPosition(PointerEventData eventData)
        {
            List<RaycastResult> results = new List<RaycastResult>();
            graphicRaycaster.Raycast(eventData, results);

            foreach (var result in results)
            {
                IPanelChannel channel = result.gameObject.GetComponent<IPanelChannel>();
                if (channel != null)
                {
                    return channel;
                }
            }
            return null;
        }

        private void _HandleRightClick(PointerEventData eventData)
        {
            List<RaycastResult> results = new List<RaycastResult>();
            graphicRaycaster.Raycast(eventData, results);

            int groupIndex = _moveState.GetSelectedGroup()?.groupIndex ?? -1;
            List<int> channelIndices = null;

            // selectChannels 확인
            if (selectChannels.Count > 0)
            {
                channelIndices = selectChannels.Select(ch => ch.channelIndex).ToList();
            }

            Vector2 localMousePosition = panelRt.InverseTransformPoint(eventData.position);

            // HierarchyPanelMenuPopup를 Show
            videoViewPanelMenuPopup.SetPosition(localMousePosition);
            videoViewPanelMenuPopup.Show(true, groupIndex, channelIndices);
        }

        private void _HandleLeftClick(PointerEventData eventData)
        {
            Vector2 localMousePosition = panelRt.InverseTransformPoint(eventData.position);

            IPanelChannel clickedChannel = GetChannelAtPosition(eventData);
            if (clickedChannel != null)
            {
                if (_moveState.IsSameChannels(clickedChannel) || _moveState.IsSameGroup(clickedChannel.parentGroup))
                {
                    _moveState.StartMove(localMousePosition);
                }
                else
                {
                    DeselectChannel(new DeSelectChannelParam(this));
                    DeselectGroup(new DeselectGroupParam(this));
                    _moveState.Deselect();
                }
            }
            else
            {
                DeselectChannel(new DeSelectChannelParam(this));
                DeselectGroup(new DeselectGroupParam(this));

                _moveState.Deselect();
                rectAreaChannelSelection.StartFindSelect();
            }
        }


        #region State 

        MoveState _moveState = new MoveState();
        private class MoveState
        {
            Vector2? startPos = null;
            IPanelGroup startGroup = null;
            List<IPanelChannel> startChannels = null;

            public void Select(IPanelGroup startGroup, List<IPanelChannel> startChannels)
            {
                this.startGroup = startGroup;
                this.startChannels = startChannels;
            }

            public void Reset()
            {
                startPos = null;
                startGroup = null;
                startChannels = null;
            }

            public void StartMove(Vector2 startPos)
            {
                this.startPos = startPos;
            }

            public Vector2 GetMovedDeleta(Vector2 currentPos)
            {
                if (startPos.HasValue)
                    return currentPos - startPos.Value;
                else
                    return Vector2.zero;
            }

            public void EndMove()
            {
                startPos = null;
            }

            public void Deselect()
            {
                startGroup = null;
                startChannels = null;
            }

            public bool IsMoving(Vector2 currentPos)
            {
                if (startPos == null)
                    return false;

                return Vector2.Distance(startPos.Value, currentPos) >= 4;
            }

            public bool IsSameGroup(IPanelGroup group)
            {
                return startGroup != null && startGroup == group;
            }

            public bool IsSameChannels(IPanelChannel channel)
            {
                return startChannels != null && startChannels.Contains(channel);
            }

            public bool IsGroupSelected()
            {
                return startGroup != null;
            }

            public bool IsChannelsSelected()
            {
                return startChannels != null && startGroup == null;
            }

            public IPanelGroup GetSelectedGroup()
            {
                return startGroup;
            }
        }

        #endregion

        #endregion

        private void _MoveDeltaGroup(IPanelGroup group, Vector2 endPos)
        {
            if (group == null)
                return;

            Vector2 offset = _moveState.GetMovedDeleta(endPos);

            List<int> indices = new List<int>();
            foreach (var ch in group.hasChannels)
            {
                indices.Add(ch.channelIndex);
            }

            MoveDeltaGroupParam param = new MoveDeltaGroupParam(this, group.groupIndex, indices, offset);

            MoveDeltaGroup(param);
        }


        /// <summary>
        /// 채널 생성
        /// </summary>
        /// <param name="position"></param>
        void _CreateChannel(Vector2 position)
        {
            CreateChannelParam param = new CreateChannelParam(this, channels.Count, position);
            CreateChannel(param);
        }

        /// <summary>
        /// 채널 삭제
        /// </summary>
        void _DeleteChannel()
        {
            List<int> indices = selectChannels.Select(ch => ch.channelIndex).ToList();

            DeselectChannel(new DeSelectChannelParam(this));
            DeleteChannel(new DeleteChannelParam(this, indices));
        }

        /// <summary>
        /// 선택된 채널들 이동
        /// </summary>
        void _MoveDeltaChannels(Vector2 endPos)
        {
            if (selectChannels.Count == 0)
                return;

            Vector2 offset = _moveState.GetMovedDeleta(endPos);

            List<int> indices = new List<int>();
            foreach (var ch in selectChannels)
            {
                indices.Add(ch.channelIndex);
            }

            MoveDeltaChannelParam param = new MoveDeltaChannelParam(this, indices, offset);
            MoveDeltaChannel(param);
        }

        /// <summary>
        /// 선택된 채널들을 그룹화
        /// </summary>
        void _MakeGroup()
        {
            int groupIndex = channelReceiver.GetGroupCount();

            List<int> indices = selectChannels.Select(ch => ch.channelIndex).ToList();

            if (channelReceiver.CanGroup(indices))
            {
                MakeGroupParam param = new MakeGroupParam(this, groupIndex, indices, IGroup.SortDirection.Left, Definitions.GetDefaultGroupName(groupIndex.ToString()));

                MakeGroup(param);
            }
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
            { eEditType.MoveChannel, param => MoveChannel((MoveChannelParam)param) },

            { eEditType.Undo, param => Undo((UndoParam)param) },

            { eEditType.MakeGroup, param => MakeGroup((MakeGroupParam)param) },
            { eEditType.SelectGroup, param => SelectGroup((SelectGroupParam)param) },
            { eEditType.DeselectGroup, param => DeselectGroup((DeselectGroupParam)param) },
            { eEditType.MoveDeltaGroup, param => MoveDeltaGroup((MoveDeltaGroupParam)param) },
            { eEditType.ChangeGroupSortDirection, param => ChangeGroupSortDirection((ChangeGroupSortDirectionParam)param) },
            { eEditType.DisableMenuPopup, param => DisableMenuPopup((DisableMenuPopupParam)param) },
            { eEditType.ReleaseGroup, param => ReleaseGroup((ReleaseGroupParam)param) },
            { eEditType.UnGroupForFree, param => UnGroupForFree((UnGroupForFreeParam)param) },
        };

        public void Apply(EditParam param)
        {
            channelSyncer.SyncAllPanel(this, param);
        }

        public void Sync(EditParam param)
        {
            syncParamMap[param.editType].Invoke(param);
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

        public void CreateChannel(CreateChannelParam param)
        {
            var ch = Instantiate(videoViewChannelPrefab, transform);
            ch.Init(param);
            channels.Add(ch);

            if (param.ownerPanel.Equals(this))
            {
                channelUpdater.CreateChannel(param);
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
                RefreshPanel(param.state.channels, param.state.groups);

                if (param.ownerPanel.Equals(this))
                {
                    Apply(param);
                }
            }
        }

        public void MakeGroup(MakeGroupParam param)
        {
            VideoViewGroup gr = Instantiate(videoViewGroupPrefab, transform);

            List<IPanelChannel> groupChannels = param.channelIndices.Select(index => channels.FirstOrDefault(ch => ch.channelIndex == index)).ToList();

            IPanelGroup.Param createParam = new IPanelGroup.Param();
            createParam.groupIndex = param.groupIndex;
            createParam.name = param.name;
            createParam.hasChannels = groupChannels;
            createParam.sortDirection = param.sortDirection;

            gr.Init(createParam);
            gr.Select();

            for(int i = 0; i < groupChannels.Count; i++)
            {
                groupChannels[i].SetGroup(gr, i);
            }

            groups.Add(gr);

            if (param.ownerPanel.Equals(this))
            {
                channelUpdater.MakeGroup(param);
                Apply(param);
            }
        }

        public void MoveChannel(MoveChannelParam param)
        {
            foreach (var index in param.indices)
            {
                channels[index].Move(param.movePos);
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
            // 기존의 채널과 그룹을 파괴
            DestroyAll();

            // 채널리시버에서 최신 채널 정보를 가져와서 업데이트
            foreach (var updatedChannel in dataChannels)
            {
                var createParam = new CreateChannelParam(this, updatedChannel.channelIndex, updatedChannel.position);
                var ch = Instantiate(videoViewChannelPrefab, transform);
                ch.Init(createParam);
                channels.Add(ch);
            }

            // 그룹도 업데이트
            foreach (var updatedGroup in dataGroups)
            {
                List<int> chIndices = updatedGroup.hasChannels.Select(ch => ch.channelIndex).ToList();

                IPanelGroup.Param createParam = new IPanelGroup.Param();
                createParam.groupIndex = updatedGroup.groupIndex;
                createParam.name = updatedGroup.name;
                createParam.hasChannels = updatedGroup.hasChannels.Select(ch => channels.FirstOrDefault(c => c.channelIndex == ch.channelIndex)).ToList();

                createParam.sortDirection = IGroup.SortDirection.Left;

                var gr = Instantiate(videoViewGroupPrefab, transform);
                gr.Init(createParam);

                for (int i = 0; i < updatedGroup.hasChannels.Count; i++)
                {
                    channels[updatedGroup.hasChannels[i].channelIndex].SetGroup(gr, i);
                }

                gr.Deselect();
                groups.Add(gr);
            }

            _moveState.Reset();
        }

        public void DeselectGroup(DeselectGroupParam param)
        {
            foreach (var gr in groups)
            {
                gr.Deselect();
            }

            foreach(var ch in channels)
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

            groups[param.groupIndex].Select();

            if (param.ownerPanel.Equals(this))
            {
                channelUpdater.MoveDeltaGroup(param);
                Apply(param);
            }
        }

        public void ChangeGroupSortDirection(ChangeGroupSortDirectionParam param)
        {
            var group = groups[param.groupIndex];
            group.sortDirection = param.sortDirection;
            group.ChnageSortDirection();
            if (param.ownerPanel.Equals(this))
            {
                channelUpdater.ChangeGroupSortDirection(param);
                Apply(param);
            }
        }

        public void DisableMenuPopup(DisableMenuPopupParam param)
        {
            videoViewPanelMenuPopup.Show(false);
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
                DLogger.LogError($"Group with index {param.groupIndex} not found.");
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

            // 5. 패널 새로고침
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

            RefreshPanel(channelReceiver.GetChannels(), channelReceiver.GetGroups());
        }

        #endregion
    }
}