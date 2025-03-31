using System.Collections.Generic;
using UnityEngine;
using static DataExtract.IExtractDataStore;


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

        public ExtractDataStoreImp()
        {
            //초기 아무것도 없는 상태를 Push
            stateStack.Push(new DatatStoreState());
        }

        #region Channel



        void _DeleteChannelBody(IChannel channel)
        {
            _channels.Remove(channel);
        }


        #endregion

        #region Group

        public void CreateGroup(IGroup.InitInfo info)
        {
            IGroup group = new Group();
            group.Create(info);
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
            IChannel channel = new Channel();

            channel.Create(param);
            _channels.Add(channel);

            StackEditParam(param);
        }

        public void MoveChannel(MoveChannelParam param)
        {
            foreach (int index in param.indices)
            {
                IChannel channel = _channels.Find(x => x.channelIndex == index);
                channel.position = param.position;
            }

            StackEditParam(param);
        }

        public void DeleteChannel(DeleteChannelParam param)
        {
            foreach (int index in param.indices)
            {
                IChannel channel = _channels.Find(x => x.channelIndex == index);
                _DeleteChannelBody(channel);
            }

            StackEditParam(param);
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
            foreach (int index in param.indices)
            {
                IChannel channel = _channels.Find(x => x.channelIndex == index);
                channel.position += param.movePos;
            }

            StackEditParam(param);
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

        #endregion
    }
}