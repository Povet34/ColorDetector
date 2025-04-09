using System;
using System.Collections.Generic;


namespace DataExtract
{
    public interface IPanelSync
    {
        Dictionary<eEditType, Action<EditParam>> syncParamMap { get; }
        void Init(ChannelUpdater channelUpdater, ChannelReceiver channelGetter, ChannelSyncer channelSyncer);
        void Apply(EditParam param);
        void Sync(EditParam param);
        void RefreshPanel(List<IChannel> dataChannels, List<IGroup> dataGroups);


        void CreateChannel(CreateChannelParam param);
        void MoveChannel(MoveChannelParam param);
        void MoveDeltaChannel(MoveDeltaChannelParam param);
        void DeleteChannel(DeleteChannelParam param);
        void SelectChannel(SelectChannelParam param);
        void DeselectChannel(DeSelectChannelParam param);
        
        
        void MakeGroup(MakeGroupParam param);
        void SelectGroup(SelectGroupParam param);


        void Undo(UndoParam param);

    }
}