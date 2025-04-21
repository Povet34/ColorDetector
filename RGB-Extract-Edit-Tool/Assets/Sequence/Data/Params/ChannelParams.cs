using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace DataExtract
{
    public enum eEditType
    {
        CreateChannel,
        MoveChannel,
        MoveDeltaChannel,
        DeleteChannel,
        SelectChannel,
        DeSelectChannel,
        
        Undo,

        MakeGroup,
        SelectGroup,
        DeselectGroup,
        MoveDeltaGroup,
        ChangeGroupSortDirection,
        
        DisableMenuPopup,
    }

    public class EditParam
    {
        public IPanelSync ownerPanel;
        public eEditType editType;

        public EditParam() { }

        public EditParam(IPanelSync ownerPanel, eEditType editType)
        {
            this.ownerPanel = ownerPanel;
            this.editType = editType;
        }
    }

    public class CreateChannelParam : EditParam
    {
        public int chIndex;
        public Vector2 createPos;

        public CreateChannelParam() { }

        public CreateChannelParam(IPanelSync ownerPanel, int chIndex, Vector2 createPos)
            : base(ownerPanel, eEditType.CreateChannel)
        {
            this.chIndex = chIndex;
            this.createPos = createPos;
        }
    }

    public class MoveChannelParam : EditParam
    {
        public List<int> indices;
        public Vector2 movePos;

        public MoveChannelParam() { }

        public MoveChannelParam(IPanelSync ownerPanel, List<int> indices, Vector2 movePos)
            : base(ownerPanel, eEditType.MoveChannel)
        {
            this.indices = indices;
            this.movePos = movePos;
        }
    }

    public class MoveDeltaChannelParam : EditParam
    {
        public List<int> indices;
        public Vector2 movePos;

        public MoveDeltaChannelParam() { }

        public MoveDeltaChannelParam(IPanelSync ownerPanel, List<int> indices, Vector2 movePos)
            : base(ownerPanel, eEditType.MoveDeltaChannel)
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

        public MoveDeltaGroupParam() { }

        public MoveDeltaGroupParam(IPanelSync ownerPanel, int groupIndex, List<int> indices, Vector2 movePos)
            : base(ownerPanel, eEditType.MoveDeltaGroup)
        {
            this.groupIndex = groupIndex;
            this.indices = indices;
            this.movePos = movePos;
        }
    }

    public class DeleteChannelParam : EditParam
    {
        public List<int> indices;

        public DeleteChannelParam() { }

        public DeleteChannelParam(IPanelSync ownerPanel, List<int> indices)
            : base(ownerPanel, eEditType.DeleteChannel)
        {
            this.indices = indices;
        }
    }

    public class SelectChannelParam : EditParam
    {
        public List<int> indices;

        public SelectChannelParam() { }

        public SelectChannelParam(IPanelSync ownerPanel, List<int> indices)
            : base(ownerPanel, eEditType.SelectChannel)
        {
            this.indices = indices;
        }
    }

    public class DeSelectChannelParam : EditParam
    {
        public DeSelectChannelParam(IPanelSync ownerPanel)
            : base(ownerPanel, eEditType.DeSelectChannel) { }
    }    
    
    
    public class UndoParam : EditParam
    {
        public IExtractDataStore.DatatStoreState state;

        public UndoParam(IPanelSync ownerPanel, IExtractDataStore.DatatStoreState state)
            : base(ownerPanel, eEditType.Undo) 
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

        public MakeGroupParam(IPanelSync ownerPanel, int groupIndex, List<int> channelIndices, IGroup.SortDirection sortDirection, string name = "NewGroup")
            : base(ownerPanel, eEditType.MakeGroup)
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
        public SelectGroupParam(IPanelSync ownerPanel, int groupIndex)
            : base(ownerPanel, eEditType.SelectGroup)
        {
            this.groupIndex = groupIndex;
        }
    }

    public class DeselectGroupParam : EditParam
    {
        public DeselectGroupParam(IPanelSync ownerPanel)
            : base(ownerPanel, eEditType.DeselectGroup)
        {
        }
    }

    public class ChangeGroupSortDirectionParam : EditParam
    {
        public int groupIndex;
        public IGroup.SortDirection sortDirection;
        public ChangeGroupSortDirectionParam(IPanelSync ownerPanel, int groupIndex, IGroup.SortDirection sortDirection)
            : base(ownerPanel, eEditType.ChangeGroupSortDirection)
        {
            this.groupIndex = groupIndex;
            this.sortDirection = sortDirection;
        }
    }

    public class DisableMenuPopupParam : EditParam
    {
        public DisableMenuPopupParam(IPanelSync ownerPanel) :
            base (ownerPanel, eEditType.DisableMenuPopup)
        {

        }
    }
}

