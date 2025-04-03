using System.Collections.Generic;
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



        void _DeleteChannelBody(IChannel channel)
        {
            _channels.Remove(channel);
        }


        #endregion

        #region Group

        public void MakeGroup(MakeGroupParam param)
        {
            IGroup group = new Group();
            group.Create(param);

            int inIndex = 0;
            foreach(var index in param.channelIndices)
            {
                var info = new IndividualInfo();
                info.parentGroup = group;
                info.inIndex = inIndex++;

                if (!channels[index].TryIncludeNewGroup(info))
                    continue;
            }

            _groups.Add(group);
        }

        public void DeleteGroup(IGroup group)
        {
            _groups.Remove(group);
        }

        #endregion

        #region EditParam

        public void CreateChannel(CreateChannelParam param)
        {
            StackEditParam(param);
            IChannel channel = new Channel();

            channel.Create(param);
            _channels.Add(channel);
        }

        public void MoveChannel(MoveChannelParam param)
        {
            StackEditParam(param);
            foreach (int index in param.indices)
            {
                IChannel channel = _channels.Find(x => x.channelIndex == index);
                channel.position = param.position;
            }
        }

        public void DeleteChannel(DeleteChannelParam param)
        {
            StackEditParam(param);
            foreach (int index in param.indices)
            {
                IChannel channel = _channels.Find(x => x.channelIndex == index);
                _DeleteChannelBody(channel);
            }
        }

        public EditParam GetLastestEditParam()
        {
            return editParamStack.Peek();
        }

        private void StackEditParam(EditParam param)
        {
            editParamStack.Push(param);
            stateStack.Push(new DatatStoreState(channels, groups));
        }

        public void MoveDeltaChannel(MoveDeltaChannelParam param)
        {
            StackEditParam(param);
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