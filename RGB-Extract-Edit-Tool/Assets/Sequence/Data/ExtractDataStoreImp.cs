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

        #region Channel

        void _SortChannels()
        {
            _channels = _channels.OrderBy(ch => ch.channelIndex).ToList();

            // channelIndex를 0부터 순차적으로 재설정
            for (int i = 0; i < _channels.Count; i++)
            {
                _channels[i].channelIndex = i;
            }
        }

        void _DeleteChannelBody(IChannel channel)
        {
            _channels.Remove(channel);
        }


        #endregion

        #region Group

        void _SortGroups()
        {
            // 삭제할 그룹을 저장할 리스트
            List<IGroup> groupsToRemove = new List<IGroup>();

            foreach (var gr in groups)
            {
                // 그룹의 채널이 channels에 존재하는지 확인하고, 존재하지 않는 채널을 제거
                gr.hasChannels = gr.hasChannels.Where(ch => _channels.Contains(ch)).ToList();

                // inIndex 순으로 정렬
                gr.hasChannels = gr.hasChannels.OrderBy(ch => ch.individualInfo.inIndex).ToList();

                // 빈 곳을 매꾸기 위해 inIndex를 재설정
                for (int i = 0; i < gr.hasChannels.Count; i++)
                {
                    gr.hasChannels[i].individualInfo.inIndex = i;
                }

                // 그룹 안에 채널이 하나도 없는 경우 그룹을 삭제할 리스트에 추가
                if (gr.hasChannels.Count == 0)
                {
                    groupsToRemove.Add(gr);
                }
                else
                {
                    // 로그 출력
                    string log = $"{gr.name}";
                    foreach (var ch in gr.hasChannels)
                    {
                        log += $"({ch.channelIndex} | {ch.individualInfo.inIndex})";
                    }
                    log += "\n";
                    DLogger.Log_Green(log);
                }
            }

            // 삭제할 그룹을 실제로 삭제
            foreach (var group in groupsToRemove)
            {
                groups.Remove(group);
            }
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
                info.parentGroup = group;
                info.inIndex = inIndex++;

                hasChannels.Add(channel);

                if (!channel.TryIncludeNewGroup(info))
                    continue;
            }

            group.Create(param, hasChannels);
            _groups.Add(group);
        }

        public void DeleteGroup(IGroup group)
        {
            _groups.Remove(group);

            _SortGroups();
            _SortChannels();
        }

        #endregion

        #region EditParam

        public void CreateChannel(CreateChannelParam param)
        {
            _StackEditParam(param);
            IChannel channel = new Channel();

            channel.Create(param);
            _channels.Add(channel);
        }

        public void MoveChannel(MoveChannelParam param)
        {
            _StackEditParam(param);
            foreach (int index in param.indices)
            {
                IChannel channel = _channels.Find(x => x.channelIndex == index);
                channel.position = param.position;
            }
        }

        public void DeleteChannel(DeleteChannelParam param)
        {
            _StackEditParam(param);
            foreach (int index in param.indices)
            {
                IChannel channel = _channels.Find(x => x.channelIndex == index);
                _DeleteChannelBody(channel);
            }

            _SortGroups();
            _SortChannels();
        }

        public EditParam GetLastestEditParam()
        {
            return editParamStack.Peek();
        }

        private void _StackEditParam(EditParam param)
        {
            editParamStack.Push(param);
            stateStack.Push(new DatatStoreState(channels, groups));
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

        public DatatStoreState GetLastestStoreState()
        {
            return stateStack.Peek();
        }

        public DatatStoreState PopLastestStoreState()
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

            if (!isCan)
                DLogger.Log_Red(log);

            return isCan;
        }

        #endregion
    }
}