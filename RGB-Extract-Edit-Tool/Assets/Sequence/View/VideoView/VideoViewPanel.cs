using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VideoViewPanel : MonoBehaviour, IPanelSync
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
