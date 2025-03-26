using System.Collections.Generic;
using UnityEngine;

public interface IPanelSync
{
    #region Param Struct

    public struct CreateChannelParam
    {
        public IPanelSync ownerPanel;
        public int chIndex;
        public Vector2 createPos;
    }

    public struct ChannelMoveParam
    {
        public IPanelSync ownerPanel;
        public int chIndex;
        public Vector2 position;
    }

    public struct DeleteChannelParam
    {
        public IPanelSync ownerPanel;
        public List<int> indexes;
    }

    #endregion

    void RegistSyncEvent();
    void UnregistSyncEvent();

    void CreateChannel(CreateChannelParam param);
    void ChannelMove(ChannelMoveParam param);
    void DeleteChannel(DeleteChannelParam param);
}
