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

        Vector2 startPos;

        void Awake()
        {
            //Channel Init
            channels = new List<IPanelChannel>();
            selectChannels = new List<IPanelChannel>();
            groups = new List<IPanelGroup>();

            //GetComp
            panelRt = GetComponent<RectTransform>();

            //MeunPopup
            videoViewPanelMenuPopup = Instantiate(videoViewPanelMenuPopupPrefab, transform);
            videoViewPanelMenuPopup.Init(
                new VideoViewPanelMenuPopup.MenuActions(
                        onCreateChannel:    _CreateChannel,
                        onCreateSegment:    null,
                        onDeleteChannel:    _DeleteChannel,
                        onMakeGroup:        _MakeGroup,
                        onReleaseGroup:     null
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

        public void Init(ChannelUpdater channelUpdater, ChannelReceiver channelReceiver, ChannelSyncer channelSyncer)
        {
            this.channelUpdater = channelUpdater;
            this.channelReceiver = channelReceiver;
            this.channelSyncer = channelSyncer;

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


        public void OnPointerDown(PointerEventData eventData)
        {
            videoViewPanelMenuPopup.Show(false);
            startPos = TransformEx.GetRelativeAnchorPosition_Screen(panelRt, eventData.position);

            //����
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                _SelectOnlyOneChannel(eventData);
                if(!_moveState.IsMoving())
                {
                    DeselectChannel(new DeSelectChannelParam(this));
                    rectAreaChannelSelection.StartFindSelect();
                }
            }

            //�޴� �˾�

            if (eventData.button == PointerEventData.InputButton.Right)
            {
                _ShowVideoViewPanelMenuPopup(startPos);
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            Vector2 endPos = TransformEx.GetRelativeAnchorPosition_Screen(panelRt, eventData.position);

            if(_moveState.IsMoving())
            {
                _MoveDeltaChannels(endPos);
                _moveState.MoveEnd();
                return;
            }

            rectAreaChannelSelection.EndFindSelect();
        }

        List<RaycastResult> GetRayHits(PointerEventData eventData)
        {
            List<RaycastResult> results = new List<RaycastResult>();
            graphicRaycaster.Raycast(eventData, results);

            return results;
        }

        /// <summary>
        /// ä�� �ϳ� ����
        /// </summary>
        /// <param name="eventData"></param>
        /// <returns></returns>
        bool _SelectOnlyOneChannel(PointerEventData eventData)
        {
            foreach (RaycastResult result in GetRayHits(eventData))
            {
                IPanelChannel selectedChannel = result.gameObject.GetComponent<IPanelChannel>();
                if (selectedChannel != null)
                {
                    if(selectedChannel.IsSelect()) //�̹� ���õ� ���̸� �̵��̴�
                    {
                        startPos = TransformEx.GetRelativeAnchorPosition_Screen(panelRt, eventData.position);
                        _moveState.MoveStart(startPos);
                    }
                    else //�����ߴ� ä���� �ƴ� ��
                    {
                        DeselectChannel(new DeSelectChannelParam(this));

                        if (selectedChannel.HasGroup()) //���õȰ� �׷��̸� �׷켱������
                        {
                            SelectGroupParam param = new SelectGroupParam(this, selectedChannel.parentGroup.groupIndex);
                            SelectGroup(param);
                        }
                        else //���õȰ� �׷��� �ƴϾ����� �׳� ���� ����
                        {
                            List<int> indices = new List<int>();
                            indices.Add(selectedChannel.channelIndex);
                            SelectChannelParam param = new SelectChannelParam(this, indices);
                            SelectChannel(param);
                        }

                    }

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Videoview panel menu popup show
        /// </summary>
        /// <param name="pos"></param>
        void _ShowVideoViewPanelMenuPopup(Vector2 pos)
        {
            if (videoViewPanelMenuPopup)
            {
                videoViewPanelMenuPopup.SetPosition(pos);
                videoViewPanelMenuPopup.Show(true);
            }
        }

        /// <summary>
        /// ä�� ����
        /// </summary>
        /// <param name="position"></param>
        void _CreateChannel(Vector2 position)
        {
            CreateChannelParam param = new CreateChannelParam(this, channels.Count, position);
            CreateChannel(param);
        }

        /// <summary>
        /// ä�� ����
        /// </summary>
        void _DeleteChannel()
        {
            List<int> indices = new List<int>();
            foreach (var ch in selectChannels)
            {
                indices.Add(ch.channelIndex);
            }

            DeselectChannel(new DeSelectChannelParam(this));
            DeleteChannel(new DeleteChannelParam(this, indices));
        }

        /// <summary>
        /// ���õ� ä�ε� �̵�
        /// </summary>
        void _MoveDeltaChannels(Vector2 endPos)
        {
            if (selectChannels.Count == 0)
                return;

            Vector2 offset = endPos - startPos;

            List<int> indices = new List<int>();
            foreach (var ch in selectChannels)
            {
                indices.Add(ch.channelIndex);
            }

            MoveDeltaChannelParam param = new MoveDeltaChannelParam(this, indices, offset);
            MoveDeltaChannel(param);
        }

        /// <summary>
        /// ���õ� ä�ε��� �׷�ȭ
        /// </summary>
        void _MakeGroup()
        {
            int groupIndex = channelReceiver.GetGroupCount();

            List<int> indices = new List<int>();
            foreach(var ch in selectChannels)
            {
                indices.Add(ch.channelIndex);
            }

            if(channelReceiver.CanGroup(indices))
            {
                MakeGroupParam param = new MakeGroupParam(this, groupIndex, indices, IGroup.SortDirection.Left, $"{groupIndex} NewGroup");

                MakeGroup(param);
            }
        }

        #region State 

        MoveState _moveState = new MoveState();

        private class MoveState
        {
            Vector2? startPos = null;
            public void MoveStart(Vector2 startPos)
            {
                this.startPos = startPos;
            }

            public void MoveEnd()
            {
                startPos = null;
            }

            public bool IsMoving()
            {
                return startPos != null;
            }
        }

        #endregion

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

            RefreshPanel();
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
                // �� ���ְ�
                DestroyAll();

                // ���� ���ġ
                // ä��
                foreach (var newCh in param.state.channels)
                {
                    var createParam = new CreateChannelParam(this, newCh.channelIndex, newCh.position);
                    var ch = Instantiate(videoViewChannelPrefab, transform);
                    ch.Init(createParam);
                    channels.Add(ch);
                }

                // �׷�
                foreach (var newGr in param.state.groups)
                {
                    List<int> chIndices = newGr.hasChannels.Select(ch => ch.channelIndex).ToList();

                    var createParam = new MakeGroupParam(this, newGr.groupIndex, chIndices, IGroup.SortDirection.Left, newGr.name);

                    var gr = Instantiate(videoViewGroupPrefab, transform);
                    gr.Init(createParam);

                    for (int i = 0; i < newGr.hasChannels.Count; i++)
                    {
                        channels[newGr.hasChannels[i].channelIndex].SetGroup(gr, i);
                    }

                    groups.Add(gr);
                }
            }

            if (param.ownerPanel.Equals(this))
            {
                Apply(param);
            }
        }



        public void MakeGroup(MakeGroupParam param)
        {
            //�׷��� �����ϰ�
            //Rect�� ���������ϳ�?
            VideoViewGroup gr = Instantiate(videoViewGroupPrefab, transform);


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
                channels[index].Move(param.position);
            }

            if (param.ownerPanel.Equals(this))
            {
                channelUpdater.MoveChannel(param);
                Apply(param);
            }
        }

        public void SelectGroup(SelectGroupParam param)
        {
            foreach(var index in groups[param.groupIndex].channelIndices)
            {
                channels[index].Select();
                selectChannels.Add(channels[index]);
            }

            if (param.ownerPanel.Equals(this))
            {
                Apply(param);
            }
        }

        public void RefreshPanel()
        {
            // ������ ä�ΰ� �׷��� �ı�
            DestroyAll();

            // ä�θ��ù����� �ֽ� ä�� ������ �����ͼ� ������Ʈ
            var updatedChannels = channelReceiver.GetChannels();
            foreach (var updatedChannel in updatedChannels)
            {
                var createParam = new CreateChannelParam(this, updatedChannel.channelIndex, updatedChannel.position);
                var ch = Instantiate(videoViewChannelPrefab, transform);
                ch.Init(createParam);
                channels.Add(ch);
            }

            // �׷쵵 ������Ʈ
            var updatedGroups = channelReceiver.GetGroups();
            foreach (var updatedGroup in updatedGroups)
            {
                List<int> chIndices = updatedGroup.hasChannels.Select(ch => ch.channelIndex).ToList();

                var createParam = new MakeGroupParam(this, updatedGroup.groupIndex, chIndices, IGroup.SortDirection.Left, updatedGroup.name);

                var gr = Instantiate(videoViewGroupPrefab, transform);
                gr.Init(createParam);

                for (int i = 0; i < updatedGroup.hasChannels.Count; i++)
                {
                    channels[updatedGroup.hasChannels[i].channelIndex].SetGroup(gr, i);
                }

                groups.Add(gr);
            }
        }


        #endregion
    }
}