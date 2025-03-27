using System.Collections.Generic;
using UnityEngine;

namespace DataExtract
{
    public interface IChannelUpdater
    {
        void Init(IExtractDataStore dataStore);
        void CreateChannel(CreateChannelParam param);
        void MoveChannel(MoveChannelParam param);
        void DeleteChannel(DeleteChannelParam param);
    }
}