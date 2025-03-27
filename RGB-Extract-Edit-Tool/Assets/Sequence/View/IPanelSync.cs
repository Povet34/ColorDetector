using System;
using System.Collections.Generic;
using UnityEngine;


namespace DataExtract
{
    public interface IPanelSync
    {
        Dictionary<eEditType, Action<EditParam>> syncParamMap { get; }
        void Init(ChannelUpdater channelUpdater, ChannelReceiver channelGetter, ChannelSyncer channelSyncer);
        void Apply(EditParam param);
        void Sync(EditParam param);

        void CreateChannel(bool isSynced, CreateChannelParam param);
        void MoveChannel(bool isSynced, MoveChannelParam param);
        void DelateChannel(bool isSynced, DeleteChannelParam param);
        void SelectChannel(bool isSynced, SelectChannelParam param);
        void DeSelectChannel(bool isSynced, DeSelectChannelParam param);
    }
}