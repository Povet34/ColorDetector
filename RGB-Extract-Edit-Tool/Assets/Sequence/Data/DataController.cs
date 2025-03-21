using System.Collections.Generic;
using UnityEngine;

public class DataController
{
    List<IChannel> channels = new List<IChannel>();
    List<IGroup> groups = new List<IGroup>();

    #region Channel

    public void CreateChannel(IChannel.InitInfo info)
    {
        IChannel channel = new Channel();
        channel.Create(info);
        channels.Add(channel);
    }

    public void DeleteChannel(IChannel channel)
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
