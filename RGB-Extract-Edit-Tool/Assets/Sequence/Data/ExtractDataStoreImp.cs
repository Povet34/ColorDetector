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

        void _SortGroupsAndChannels()
        {
            // 1. 기존 인덱스와 새로운 인덱스 매핑 생성
            Dictionary<int, int> indexMapping = new Dictionary<int, int>();
            for (int i = 0; i < _channels.Count; i++)
            {
                indexMapping[_channels[i].channelIndex] = -1; // 초기값은 -1로 설정
            }

            // 2. 채널 정렬
            _channels = _channels.OrderBy(ch => ch.channelIndex).ToList();

            // 3. 이전 채널들이 어떻게 변화했는지 체크,
            for (int i = 0; i < _channels.Count; i++)
            {
                indexMapping[_channels[i].channelIndex] = i;
            }

            // 4. 그룹 정리
            List<IGroup> groupsToRemove = new List<IGroup>();
            int groupIndex = 0;

            foreach (var gr in _groups)
            {
                // 그룹 내 채널을 매핑 정보를 기반으로 업데이트
                gr.hasChannels = gr.hasChannels
                    .Select(ch =>
                    {
                        if (indexMapping.ContainsKey(ch.channelIndex))
                        {
                            int newIndex = indexMapping[ch.channelIndex];
                            return newIndex != -1 ? _channels[newIndex] : null; // -1이면 제거 대상
                        }
                        return null;
                    })
                    .Where(ch => ch != null) // 유효하지 않은 채널 제거
                    .ToList();

                // 그룹 내 채널을 inIndex 순으로 정렬
                gr.hasChannels = gr.hasChannels.OrderBy(ch => ch.individualInfo.inIndex).ToList();

                // 그룹 내 채널의 inIndex를 재설정
                for (int i = 0; i < gr.hasChannels.Count; i++)
                {
                    gr.hasChannels[i].RedefineInGroupInIndex(i);
                }

                // 그룹 안에 채널이 하나도 없는 경우 삭제 대상에 추가
                if (gr.hasChannels.Count == 0)
                {
                    groupsToRemove.Add(gr);
                }
                else
                {
                    // 그룹의 인덱스를 재설정
                    gr.groupIndex = groupIndex++;
                }
            }

            // 5. 삭제할 그룹 제거
            foreach (var group in groupsToRemove)
            {
                _groups.Remove(group);
            }

            // 6. 마지막으로 채널들 순서 갱신
            for (int i = 0; i < _channels.Count; i++)
            {
                _channels[i].channelIndex = i;
            }
        }

        #region EditParam
        public void ChangeGroupSortDirection(ChangeGroupSortDirectionParam param)
        {
            // 변경 사항을 스택에 저장
            _StackEditParam(param);

            // 그룹을 찾음
            IGroup group = _groups.Find(x => x.groupIndex == param.groupIndex);
            if (group == null)
            {
                Debug.LogError($"Group with index {param.groupIndex} not found.");
                return;
            }

            // 그룹의 정렬 방향 변경
            group.sortDirection = param.sortDirection;

            // 그룹 내 채널 정렬
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

            // 그룹 내 인덱스 재정의
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
                    Debug.LogError($"Channel with index {index} not found.");
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
            // 1. 채널의 그룹 정보 제거
            foreach (var channelIndex in param.channelIndices)
            {
                IChannel channel = _channels.FirstOrDefault(ch => ch.channelIndex == channelIndex);
                if (channel == null)
                {
                    Debug.LogError($"Channel with index {channelIndex} not found.");
                    continue;
                }

                // 그룹 정보 제거
                channel.ExcludeGroup();
            }

            // 2. 그룹에서 채널 제거
            foreach (var group in _groups)
            {
                group.hasChannels = group.hasChannels
                    .Where(ch => !param.channelIndices.Contains(ch.channelIndex))
                    .ToList();
            }

            // 3. 채널이 없는 그룹 삭제
            _groups = _groups.Where(gr => gr.hasChannels.Count > 0).ToList();

            // 4. 상태 동기화
            _SortGroupsAndChannels();
        }

        #endregion
    }
}