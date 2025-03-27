using System.Collections.Generic;
using UnityEngine;


namespace DataExtract
{
    public class ExtractDataStoreImp : IExtractDataStore
    {
        List<IChannel> _channels = new List<IChannel>();
        List<IGroup> _groups = new List<IGroup>();

        Stack<EditParam> editParamStack = new Stack<EditParam>();

        public List<IChannel> channels { get => channels; set => channels = value; }
        public List<IGroup> groups { get => _groups; set => _groups = value; }

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

        public void SelectChannel(SelectChannelParam param)
        {
            StackEditParam(param);
        }

        public void CreateChannel(CreateChannelParam param)
        {
            IChannel channel = new Channel();

            channel.Create(param);
            _channels.Add(channel);

            StackEditParam(param);
        }

        public void MoveChannel(MoveChannelParam param)
        {
            IChannel channel = _channels.Find(x => x.channelIndex == param.chIndex);
            channel.position = param.position;

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
        }

        public void DeSelectChannel(DeSelectChannelParam param)
        {
            editParamStack.Push(param);
        }

        #endregion
    }
}