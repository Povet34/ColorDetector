using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

namespace DataExtract
{
    public interface IChannelReceiver
    {
        void Init(IExtractDataStore dataStore);
        EditParam GetLastestEditParam();
    }
}