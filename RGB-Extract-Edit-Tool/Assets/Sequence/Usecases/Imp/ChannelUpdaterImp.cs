using UnityEngine;

public class ChannelUpdaterImp : IChannelUpdater
{
    IChannelDataStore _dataStore;
    public void Init(IChannelDataStore dataStore)
    {
        _dataStore = dataStore;
    }

    public void CreateChannel(IChannelUpdater.CreateChannelParam param)
    {
        throw new System.NotImplementedException();
    }

    public void DeleteChannel(IChannelUpdater.DeleteChannelParam param)
    {
        throw new System.NotImplementedException();
    }

    public void MoveChannel(IChannelUpdater.ChannelMoveParam param)
    {
        throw new System.NotImplementedException();
    }
}
