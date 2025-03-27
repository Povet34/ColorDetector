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

        public void Init(IExtractDataStore dataStore)
        {
            _dataStore = dataStore;
        }
    }
}