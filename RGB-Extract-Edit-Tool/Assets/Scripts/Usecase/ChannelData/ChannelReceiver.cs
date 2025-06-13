using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DataExtract
{
    public class ChannelReceiver
    {
        IExtractDataStore _dataStore;

        public ChannelReceiver(IExtractDataStore dataStore)
        {
            _dataStore = dataStore;
        }

        public int GetGroupCount()
        {
            return _dataStore.GetGroupCount();
        }

        public bool CanGroup(List<int> indices)
        {
            return _dataStore.CanGroup(indices);
        }

        public List<IChannel> GetChannels()
        {
            return _dataStore.channels;
        }

        public List<IGroup> GetGroups()
        {
            return _dataStore.groups;
        }

        public Dictionary<int, List<Color32>> GetExtractData()
        {
            return _dataStore.extractMap;
        }

        public IChannel GetChannel(int channelIndex)
        {
            return _dataStore.channels.FirstOrDefault(c => c.channelIndex == channelIndex);
        }
    }
}