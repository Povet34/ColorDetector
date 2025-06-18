using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static DataExtract.IExtractDataStore;
using static IChannel;


namespace DataExtract
{
    public class ExtractDataStoreImp : IExtractDataStore
    {
        List<IChannel> _channels = new List<IChannel>();
        List<IGroup> _groups = new List<IGroup>();

        Stack<EditParam> editParamStack = new Stack<EditParam>();
        Stack<DatatStoreState> stateStack = new Stack<DatatStoreState>();

        public List<IChannel> channels { get => _channels; set => _channels = value; }
        public List<IGroup> groups { get => _groups; set => _groups = value; }
        public Dictionary<int, List<Color32>> extractMap { get; set; }
        public Vector2 videoResolution { get; set; }

        void _SortGroupsAndChannels()
        {
            // 1. ���� �ε����� ���ο� �ε��� ���� ����
            Dictionary<int, int> indexMapping = new Dictionary<int, int>();
            for (int i = 0; i < _channels.Count; i++)
            {
                indexMapping[_channels[i].channelIndex] = -1; // �ʱⰪ�� -1�� ����
            }

            // 2. ä�� ����
            _channels = _channels.OrderBy(ch => ch.channelIndex).ToList();

            // 3. ���� ä�ε��� ��� ��ȭ�ߴ��� üũ,
            for (int i = 0; i < _channels.Count; i++)
            {
                indexMapping[_channels[i].channelIndex] = i;
            }

            // 4. �׷� ����
            List<IGroup> groupsToRemove = new List<IGroup>();
            int groupIndex = 0;

            foreach (var gr in _groups)
            {
                // �׷� �� ä���� ���� ������ ������� ������Ʈ
                gr.hasChannels = gr.hasChannels
                    .Select(ch =>
                    {
                        if (indexMapping.ContainsKey(ch.channelIndex))
                        {
                            int newIndex = indexMapping[ch.channelIndex];
                            return newIndex != -1 ? _channels[newIndex] : null; // -1�̸� ���� ���
                        }
                        return null;
                    })
                    .Where(ch => ch != null) // ��ȿ���� ���� ä�� ����
                    .ToList();

                // �׷� �� ä���� inIndex ������ ����
                gr.hasChannels = gr.hasChannels.OrderBy(ch => ch.individualInfo.inIndex).ToList();

                // �׷� �� ä���� inIndex�� �缳��
                for (int i = 0; i < gr.hasChannels.Count; i++)
                {
                    gr.hasChannels[i].RedefineInGroupInIndex(i);
                }

                // �׷� �ȿ� ä���� �ϳ��� ���� ��� ���� ��� �߰�
                if (gr.hasChannels.Count == 0)
                {
                    groupsToRemove.Add(gr);
                }
                else
                {
                    // �׷��� �ε����� �缳��
                    gr.groupIndex = groupIndex++;
                }
            }

            // 5. ������ �׷� ����
            foreach (var group in groupsToRemove)
            {
                _groups.Remove(group);
            }

            // 6. ���������� ä�ε� ���� ����
            for (int i = 0; i < _channels.Count; i++)
            {
                _channels[i].channelIndex = i;
            }
        }

        #region EditParam
        public void ChangeGroupSortDirection(ChangeGroupSortDirectionParam param)
        {
            // ���� ������ ���ÿ� ����
            _StackEditParam(param);

            // �׷��� ã��
            IGroup group = _groups.Find(x => x.groupIndex == param.groupIndex);
            if (group == null)
            {
                DLogger.LogError($"Group with index {param.groupIndex} not found.");
                return;
            }

            // �׷��� ���� ���� ����
            group.sortDirection = param.sortDirection;

            // �׷� �� ä�� ����
            switch (group.sortDirection)
            {
                case IGroup.SortDirection.Left:
                    group.hasChannels = group.hasChannels.OrderByDescending(ch => ch.position.x).ToList();
                    break;
                case IGroup.SortDirection.Right:
                    group.hasChannels = group.hasChannels.OrderBy(ch => ch.position.x).ToList();
                    break;
                case IGroup.SortDirection.Up:
                    group.hasChannels = group.hasChannels.OrderBy(ch => ch.position.y).ToList();
                    break;
                case IGroup.SortDirection.Down:
                    group.hasChannels = group.hasChannels.OrderByDescending(ch => ch.position.y).ToList();
                    break;
            }

            // �׷� �� �ε��� ������
            for (int i = 0; i < group.hasChannels.Count; i++)
            {
                group.hasChannels[i].RedefineInGroupInIndex(i);
            }
        }

        public void CreateChannel(CreateChannelParam param)
        {
            _StackEditParam(param);
            IChannel channel = new Channel();

            channel.Create(param);
            _channels.Add(channel);

            _SortGroupsAndChannels();
        }

        public void MoveChannel(MoveChannelParam param)
        {
            _StackEditParam(param);
            foreach (int index in param.indices)
            {
                IChannel channel = _channels.Find(x => x.channelIndex == index);
                channel.position = param.movePos;
            }
        }

        public void DeleteChannel(DeleteChannelParam param)
        {
            _StackEditParam(param);
            foreach (int index in param.indices)
            {
                IChannel channel = _channels.Find(x => x.channelIndex == index);
                _channels.Remove(channel);
            }

            _SortGroupsAndChannels();
        }

        private void _StackEditParam(EditParam param)
        {
            editParamStack.Push(param);
            stateStack.Push(new DatatStoreState(channels, groups));
        }

        public void MakeGroup(MakeGroupParam param)
        {
            _StackEditParam(param);
            IGroup group = new Group();

            int inIndex = 0;
            List<IChannel> hasChannels = new List<IChannel>();

            foreach (var index in param.channelIndices)
            {
                IChannel channel = _channels.Find(ch => ch.channelIndex == index);
                if (channel == null)
                {
                    DLogger.LogError($"Channel with index {index} not found.");
                    continue;
                }

                var info = new IndividualInfo();
                info.Redefine(group, inIndex++);

                hasChannels.Add(channel);

                if (!channel.TryIncludeNewGroup(info))
                    continue;
            }

            group.Create(param, hasChannels);
            _groups.Add(group);
        }

        public void MoveDeltaChannel(MoveDeltaChannelParam param)
        {
            _StackEditParam(param);
            foreach (int index in param.indices)
            {
                IChannel channel = _channels.Find(x => x.channelIndex == index);
                channel.position += param.movePos;
            }
        }

        private DatatStoreState PopLastestStoreState()
        {
            return stateStack.Pop();
        }

        public DatatStoreState Undo()
        {
            if (stateStack.Count == 0)
            {
                DLogger.Log_Red("StateStack is null");
                return null;
            }

            var state = PopLastestStoreState();
            channels = state.channels;
            groups = state.groups;

            return state;
        }

        public int GetGroupCount()
        {
            return groups.Count;
        }

        public bool CanGroup(List<int> indices)
        {
            bool isCan = true;
            string log = "";

            foreach(var index in indices)
            {
                if (channels[index].IsIncludedGroup())
                {
                    isCan = false;
                    log += $"Origin : {channels[index].channelIndex} - GroupName : {channels[index].individualInfo.parentGroup.name} : InIndex : {channels[index].individualInfo.inIndex} \n";
                }
            }

            return isCan;
        }

        public void MoveDeltaGroup(MoveDeltaGroupParam param)
        {
            _StackEditParam(param);

            foreach (var group in _groups)
            {
                foreach (var channel in group.hasChannels)
                {
                    if (param.indices.Contains(channel.channelIndex))
                    {
                        channel.position += param.movePos;
                    }
                }
            }
        }

        public void ReleaseGroup(ReleaseGroupParam param)
        {
            _StackEditParam(param);
            IGroup group = null;
            foreach (var gr in _groups)
            {
                if (gr.groupIndex == param.groupIndex)
                {
                    group = gr;
                    break;
                }
            }

            group.ReleaseGroup();
            _groups.Remove(group);

            _SortGroupsAndChannels();
        }

        public void UnGroupForFree(UnGroupForFreeParam param)
        {
            // 1. ä���� �׷� ���� ����
            foreach (var channelIndex in param.channelIndices)
            {
                IChannel channel = _channels.FirstOrDefault(ch => ch.channelIndex == channelIndex);
                if (channel == null)
                {
                    DLogger.LogError($"Channel with index {channelIndex} not found.");
                    continue;
                }

                // �׷� ���� ����
                channel.ExcludeGroup();
            }

            // 2. �׷쿡�� ä�� ����
            foreach (var group in _groups)
            {
                group.hasChannels = group.hasChannels
                    .Where(ch => !param.channelIndices.Contains(ch.channelIndex))
                    .ToList();
            }

            // 3. ä���� ���� �׷� ����
            _groups = _groups.Where(gr => gr.hasChannels.Count > 0).ToList();

            // 4. ���� ����ȭ
            _SortGroupsAndChannels();
        }


        public void StoreExtractData(Texture2D texture)
        {
            if (texture == null)
            {
                DLogger.LogError("Texture2D is null. Cannot extract.");
                return;
            }

            foreach (var channel in channels)
            {
                // ä���� ������ ��������
                Vector2 position = channel.position;

                // �������� �ؽ�ó ��ǥ�� ��ȯ
                int x = Mathf.Clamp((int)position.x, 0, texture.width - 1);
                int y = Mathf.Clamp((int)position.y, 0, texture.height - 1);

                // �ؽ�ó���� �ش� ��ġ�� �ȼ� ���� ��������
                Color pixelColor = texture.GetPixel(x, y);
                Color32 pixelColor32 = (Color32)pixelColor;

                // extractMap�� ä���� ���� ������ �߰�
                if (!extractMap.ContainsKey(channel.channelIndex))
                {
                    extractMap[channel.channelIndex] = new List<Color32>();
                }

                extractMap[channel.channelIndex].Add(pixelColor32);
            }

        }

        public void StoreStart()
        {
            extractMap = new Dictionary<int, List<Color32>>();
        }

        public void RenameGroup(RenameGroupParam param)
        {
            _StackEditParam(param);
            IGroup group = _groups.Find(x => x.groupIndex == param.groupIndex);
            if (group == null)
            {
                DLogger.LogError($"Group with index {param.groupIndex} not found.");
                return;
            }
            group.Rename(param);
        }

        #endregion

        private Vector2 ArrangeChannelPositions(Vector2 oldPos)
        {
            // videoViewPanelSize�� ũ��
            Vector2 videoViewPanelSize = new Vector2(1520, 940);
            oldPos += videoViewPanelSize / 2f;

            // videoResolution�� ũ��
            Vector2 videoResolution = this.videoResolution;

            // ���� ��� (videoResolution�� videoViewPanelSize�� �°� �����ϸ�)
            float scaleX = videoViewPanelSize.x / videoResolution.x;
            float scaleY = videoViewPanelSize.y / videoResolution.y;

            // oldPos�� �����ϸ��Ͽ� ���ο� ��ġ ���
            Vector2 newPos = new Vector2(
                oldPos.x * scaleX,
                oldPos.y * scaleY
            );

            return newPos;
        }

        public void ResetAll(ResetAllParam param)
        {
            _StackEditParam(param);

            _channels.Clear();
            _groups.Clear();
        }
    }
}