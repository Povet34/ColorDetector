using System.Collections.Generic;
using UnityEngine;

public interface IPanelSync
{
    void Init(
        IChannelDataStore channelDataStore, 
        IChannelUpdater channelUpdater, 
        IChannelGetter channelGetter
        );
}
