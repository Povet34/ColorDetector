using System.Collections.Generic;
using UnityEngine;

public interface IChannelUpdater
{
    #region Params

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

    void Init(IChannelDataStore dataStore);
    void CreateChannel(CreateChannelParam param);
    void MoveChannel(ChannelMoveParam param);
    void DeleteChannel(DeleteChannelParam param);
}
