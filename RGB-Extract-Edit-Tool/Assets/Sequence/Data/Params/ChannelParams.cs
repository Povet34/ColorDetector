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
        public Vector2 position;

        public MoveChannelParam() { }

        public MoveChannelParam(IPanelSync ownerPanel, List<int> indices, Vector2 position)
            : base(ownerPanel, eEditType.MoveChannel)
        {
            this.indices = indices;
            this.position = position;
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

        public MakeGroupParam(IPanelSync ownerPanel, int groupIndex, List<int> channels, IGroup.SortDirection sortDirection, string name = "NewGroup")
            : base(ownerPanel, eEditType.MakeGroup)
        {
            this.groupIndex = groupIndex;
            this.channelIndices = channels;
            this.sortDirection = sortDirection;
            this.name = name;
        }
    }
}

