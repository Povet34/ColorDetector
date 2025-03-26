using System.Collections.Generic;
using UnityEngine;

public class DataController
{
    List<IChannel> channels = new List<IChannel>();
    List<IGroup> groups = new List<IGroup>();

    #region Channel

    public void CreateChannel(int index, Vector2 position)
    {
        IChannel.InitInfo initInfo = new IChannel.InitInfo();
        initInfo.channelIndex = index;
        initInfo.position = position;

        IChannel channel = new Channel();

        channel.Create(initInfo);
        channels.Add(channel);
    }

    public void MoveChannel(int index, Vector2 position)
    {
        IChannel channel = channels.Find(x => x.channelIndex == index);
        channel.position = position;
    }

    public void DeleteChannel(List<int> indexes)
    {
        foreach (int index in indexes)
        {
            IChannel channel = channels.Find(x => x.channelIndex == index);
            _DeleteChannelBody(channel);
        }
    }

    void _DeleteChannelBody(IChannel channel)
    {
        channels.Remove(channel);
    }

    #endregion

    #region Group

    public void CreateGroup(IGroup.InitInfo info)
    {
        IGroup group = new Group();
        group.Create(info);
        groups.Add(group);
    }

    public void DeleteGroup(IGroup group)
    {
        groups.Remove(group);
    }

    #endregion
}
