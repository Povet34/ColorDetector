using System;
using System.Collections.Generic;
using UnityEngine;


namespace DataExtract
{
    public interface IPanelSync
    {
        void Init(ChannelUpdater channelUpdater, ChannelReceiver channelGetter, ChannelSyncer channelSyncer);
        void Apply(EditParam param);
        void Sync(EditParam param);

        void CreateChannel(bool isSync, CreateChannelParam param);
        void MoveChannel(bool isSync, MoveChannelParam param);
        void DelateChannel(bool isSync, DeleteChannelParam param);
    }
}