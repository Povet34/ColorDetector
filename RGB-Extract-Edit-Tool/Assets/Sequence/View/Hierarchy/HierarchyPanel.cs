using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class HierarchyPanel : MonoBehaviour, IPanelSync
{
    IChannelDataStore dataStore;
    IChannelUpdater IChannelUpdater;
    IChannelGetter IChannelGetter;

    List<IPanelChannel> channels;

    public void Init(IChannelDataStore channelDataStore, IChannelUpdater channelUpdater, IChannelGetter channelGetter)
    {
        dataStore = channelDataStore;
        IChannelUpdater = channelUpdater;
        IChannelGetter = channelGetter;

        channels = new List<IPanelChannel>();
    }
}
