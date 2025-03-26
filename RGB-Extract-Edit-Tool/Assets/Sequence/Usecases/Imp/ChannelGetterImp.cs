using System.Collections.Generic;
using UnityEngine;

public class ChannelGetterImp : IChannelGetter
{
    IChannelDataStore _dataStore;

    public void Init(IChannelDataStore dataStore)
    {
        _dataStore = dataStore;
    }

    public List<IChannel> GetChannels()
    {
        throw new System.NotImplementedException();
    }

    public List<IGroup> GetGroups()
    {
        throw new System.NotImplementedException();
    }
}
