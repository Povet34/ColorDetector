using System;
using System.Collections.Generic;


namespace DataExtract
{
    public interface IPanelSync
    {
        Dictionary<eEditType, Action<EditParam>> syncParamMap { get; }
        void Init(DataExtractMain.PanelInjection injection);
        void Refresh(RefreshParam param);
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
        void DeselectGroup(DeselectGroupParam param);
        void MoveDeltaGroup(MoveDeltaGroupParam param);
        void ChangeGroupSortDirection(ChangeGroupSortDirectionParam param);
        void ReleaseGroup(ReleaseGroupParam param);
        void UnGroupForFree(UnGroupForFreeParam param);
        void RenameGroup(RenameGroupParam param);

        void DisableMenuPopup(DisableMenuPopupParam param);
        void Undo(UndoParam param);
    }
}