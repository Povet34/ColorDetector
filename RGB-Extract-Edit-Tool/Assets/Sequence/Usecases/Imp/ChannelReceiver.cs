using System.Collections.Generic;
using UnityEngine;

namespace DataExtract
{
    public class ChannelReceiver
    {
        IExtractDataStore _dataStore;

        public EditParam GetLastestEditParam()
        {
            return _dataStore.GetLastestEditParam();
        }

        public IExtractDataStore.DatatStoreState PopLastestStoreState()
        {
            return _dataStore.PopLastestStoreState();
        }

        public IExtractDataStore.DatatStoreState GetLastestStoreState()
        {
            return _dataStore.GetLastestStoreState();
        }

        public void Init(IExtractDataStore dataStore)
        {
            _dataStore = dataStore;
        }
    }
}