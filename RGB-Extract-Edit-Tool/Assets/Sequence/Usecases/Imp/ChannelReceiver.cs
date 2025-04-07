using System.Collections.Generic;
using UnityEngine;

namespace DataExtract
{
    public class ChannelReceiver
    {
        IExtractDataStore _dataStore;

        public void Init(IExtractDataStore dataStore)
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
    }
}