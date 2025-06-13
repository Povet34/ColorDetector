using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace DataExtract
{
    public enum eEditType
    {
        Refresh,

        CreateChannel,
        MoveChannel,
        MoveDeltaChannel,
        DeleteChannel,
        SelectChannel,
        DeSelectChannel,

        Undo,
        ResetAll,

        MakeGroup,
        SelectGroup,
        DeselectGroup,
        MoveDeltaGroup,
        ChangeGroupSortDirection,
        ReleaseGroup,
        UnGroupForFree,
        RenameGroup,

        DisableMenuPopup,
    }

    public class EditParam
    {
        public IPanelSync ownerPanel;
        public eEditType editType;
        public bool wantStacked = true; // If true, this edit will be added to the undo stack

        public EditParam() { }

        public EditParam(IPanelSync ownerPanel, eEditType editType, bool wantStacked)
        {
            this.ownerPanel = ownerPanel;
            this.editType = editType;
            this.wantStacked = wantStacked;
        }
    }

    public class RefreshParam : EditParam
    {
        public RefreshParam(IPanelSync ownerPanel = null, bool wantStacked = true)
            : base(ownerPanel, eEditType.Refresh, wantStacked)
        {
        }
    }

    public class CreateChannelParam : EditParam
    {
        public int chIndex;
        public Vector2 createPos;
        public CreateChannelParam(IPanelSync ownerPanel, int chIndex, Vector2 createPos, bool wantStacked = true)
            : base(ownerPanel, eEditType.CreateChannel, wantStacked)
        {
            this.chIndex = chIndex;
            this.createPos = createPos;
        }
    }

    public class MoveChannelParam : EditParam
    {
        public List<int> indices;
        public Vector2 movePos;
        public MoveChannelParam(IPanelSync ownerPanel, List<int> indices, Vector2 movePos, bool wantStacked = true)
            : base(ownerPanel, eEditType.MoveChannel, wantStacked)
        {
            this.indices = indices;
            this.movePos = movePos;
        }
    }

    public class MoveDeltaChannelParam : EditParam
    {
        public List<int> indices;
        public Vector2 movePos;
        public MoveDeltaChannelParam(IPanelSync ownerPanel, List<int> indices, Vector2 movePos, bool wantStacked = true)
            : base(ownerPanel, eEditType.MoveDeltaChannel, wantStacked)
        {
            this.indices = indices;
            this.movePos = movePos;
        }
    }

    public class MoveDeltaGroupParam : EditParam
    {
        public int groupIndex;
        public List<int> indices;
        public Vector2 movePos;
        public MoveDeltaGroupParam(IPanelSync ownerPanel, int groupIndex, List<int> indices, Vector2 movePos, bool wantStacked = true)
            : base(ownerPanel, eEditType.MoveDeltaGroup, wantStacked)
        {
            this.groupIndex = groupIndex;
            this.indices = indices;
            this.movePos = movePos;
        }
    }

    public class DeleteChannelParam : EditParam
    {
        public List<int> indices;
        public DeleteChannelParam(IPanelSync ownerPanel, List<int> indices, bool wantStacked = true)
            : base(ownerPanel, eEditType.DeleteChannel, wantStacked)
        {
            this.indices = indices;
        }
    }

    public class SelectChannelParam : EditParam
    {
        public List<int> indices;

        public SelectChannelParam(IPanelSync ownerPanel, List<int> indices, bool wantStacked = true)
            : base(ownerPanel, eEditType.SelectChannel, wantStacked)
        {
            this.indices = indices;
        }
    }

    public class DeSelectChannelParam : EditParam
    {
        public DeSelectChannelParam(IPanelSync ownerPanel, bool wantStacked = true)
            : base(ownerPanel, eEditType.DeSelectChannel, wantStacked) { }
    }


    public class UndoParam : EditParam
    {
        public IExtractDataStore.DatatStoreState state;

        public UndoParam(IPanelSync ownerPanel, IExtractDataStore.DatatStoreState state, bool wantStacked = true)
            : base(ownerPanel, eEditType.Undo, wantStacked)
        {
            this.state = state;
        }
    }

    public class MakeGroupParam : EditParam
    {
        public int groupIndex;
        public List<int> channelIndices;
        public IGroup.SortDirection sortDirection;
        public string name;

        public MakeGroupParam(IPanelSync ownerPanel, int groupIndex, List<int> channelIndices, IGroup.SortDirection sortDirection, string name = "NewGroup", bool wantStacked = true)
            : base(ownerPanel, eEditType.MakeGroup, wantStacked)
        {
            this.groupIndex = groupIndex;
            this.channelIndices = channelIndices;
            this.sortDirection = sortDirection;
            this.name = name;
        }
    }

    public class SelectGroupParam : EditParam
    {
        public int groupIndex;
        public SelectGroupParam(IPanelSync ownerPanel, int groupIndex, bool wantStacked = true)
            : base(ownerPanel, eEditType.SelectGroup, wantStacked)
        {
            this.groupIndex = groupIndex;
        }
    }

    public class DeselectGroupParam : EditParam
    {
        public DeselectGroupParam(IPanelSync ownerPanel, bool wantStacked = true)
            : base(ownerPanel, eEditType.DeselectGroup, wantStacked)
        {
        }
    }

    public class ChangeGroupSortDirectionParam : EditParam
    {
        public int groupIndex;
        public IGroup.SortDirection sortDirection;
        public ChangeGroupSortDirectionParam(IPanelSync ownerPanel, int groupIndex, IGroup.SortDirection sortDirection, bool wantStacked = true)
            : base(ownerPanel, eEditType.ChangeGroupSortDirection, wantStacked)
        {
            this.groupIndex = groupIndex;
            this.sortDirection = sortDirection;
        }
    }

    public class DisableMenuPopupParam : EditParam
    {
        public DisableMenuPopupParam(IPanelSync ownerPanel, bool wantStacked = true) :
            base(ownerPanel, eEditType.DisableMenuPopup, wantStacked)
        {
        }
    }

    public class ReleaseGroupParam : EditParam
    {
        public int groupIndex;
        public ReleaseGroupParam(IPanelSync ownerPanel, int groupIndex, bool wantStacked = true)
            : base(ownerPanel, eEditType.ReleaseGroup, wantStacked)
        {
            this.groupIndex = groupIndex;
        }
    }

    public class UnGroupForFreeParam : EditParam
    {
        public List<int> channelIndices;
        public UnGroupForFreeParam(IPanelSync ownerPanel, List<int> channelIndices, bool wantStacked = true)
            : base(ownerPanel, eEditType.UnGroupForFree, wantStacked)
        {
            this.channelIndices = channelIndices;
        }
    }

    public class RenameGroupParam : EditParam
    {
        public int groupIndex;
        public string newName;

        public RenameGroupParam(IPanelSync ownerPanel, int groupIndex, string newName, bool wantStacked = true)
            : base(ownerPanel, eEditType.RenameGroup, wantStacked)
        {
            this.groupIndex = groupIndex;
            this.newName = newName;
        }
    }

    public class ResetAllParam : EditParam
    {
        public ResetAllParam(IPanelSync ownerPanel = null, bool wantStacked = true) 
            : base(ownerPanel, eEditType.ResetAll, wantStacked)
        {
        }
    }
}

