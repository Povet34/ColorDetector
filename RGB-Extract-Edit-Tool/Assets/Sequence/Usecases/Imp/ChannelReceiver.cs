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
    }
}