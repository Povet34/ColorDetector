using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public interface IChannelGetter
{
    void Init(IChannelDataStore dataStore);
    List<IChannel> GetChannels();
    List<IGroup> GetGroups();
}
