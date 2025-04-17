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

            // channelIndex�� 0���� ���������� �缳��
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
            // ������ �׷��� ������ ����Ʈ
            List<IGroup> groupsToRemove = new List<IGroup>();
            int groupIndex = 0;

            foreach (var gr in groups)
            {
                // �׷��� ä���� channels�� �����ϴ��� Ȯ���ϰ�, �������� �ʴ� ä���� ����
                gr.hasChannels = gr.hasChannels.Where(ch => _channels.Any(c => c.channelIndex == ch.channelIndex)).ToList();

                // inIndex ������ ����
                gr.hasChannels = gr.hasChannels.OrderBy(ch => ch.individualInfo.inIndex).ToList();

                // �� ���� �Ųٱ� ���� inIndex�� �缳��
                for (int i = 0; i < gr.hasChannels.Count; i++)
                {
                    gr.hasChannels[i].individualInfo.inIndex = i;
                }

                // �׷� �ȿ� ä���� �ϳ��� ���� ��� �׷��� ������ ����Ʈ�� �߰�
                if (gr.hasChannels.Count == 0)
                {
                    groupsToRemove.Add(gr);
                }
                else
                {
                    // �α� ���
                    string log = $"{gr.name}";
                    foreach (var ch in gr.hasChannels)
                    {
                        log += $"({ch.channelIndex} | {ch.individualInfo.inIndex})";
                    }
                    log += "\n";
                    DLogger.Log_Green(log);

                    gr.groupIndex = groupIndex++;
                }
            }

            // ������ �׷��� ������ ����
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
                channel.position = param.movePos;
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

        #endregion
    }
}