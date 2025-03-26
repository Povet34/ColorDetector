using System.Collections.Generic;
using UnityEngine;

public interface IChannelDataStore
{
    List<IChannel> channels { get; set; }
    List<IGroup> groups { get; set; }

    #region Channel

    public void CreateChannel(int index, Vector2 position);
    public void MoveChannel(int index, Vector2 position);
    public void DeleteChannel(List<int> indexes);

    #endregion

    #region Group

    public void CreateGroup(IGroup.InitInfo info);
    public void DeleteGroup(IGroup group);

    #endregion
}
