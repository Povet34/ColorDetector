using System.Collections.Generic;
using UnityEngine;

public class ChannelDataStoreImp : IChannelDataStore
{
    List<IChannel> _channels = new List<IChannel>();
    List<IGroup> _groups = new List<IGroup>();

    public List<IChannel> channels { get => channels; set => channels = value; }
    public List<IGroup> groups { get => _groups; set => _groups = value; }

    #region Channel

    public void CreateChannel(int index, Vector2 position)
    {
        IChannel.InitInfo initInfo = new IChannel.InitInfo();
        initInfo.channelIndex = index;
        initInfo.position = position;

        IChannel channel = new Channel();

        channel.Create(initInfo);
        _channels.Add(channel);
    }

    public void MoveChannel(int index, Vector2 position)
    {
        IChannel channel = _channels.Find(x => x.channelIndex == index);
        channel.position = position;
    }

    public void DeleteChannel(List<int> indexes)
    {
        foreach (int index in indexes)
        {
            IChannel channel = _channels.Find(x => x.channelIndex == index);
            _DeleteChannelBody(channel);
        }
    }

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
}
