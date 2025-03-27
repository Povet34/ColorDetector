using System.Collections.Generic;
using UnityEngine;

namespace DataExtract
{
    public enum eEditType
    {
        CreateChannel,
        MoveChannel,
        DeleteChannel,
        SelectChannel
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

        public CreateChannelParam(IPanelSync ownerPanel, eEditType editType, int chIndex, Vector2 createPos)
            : base(ownerPanel, eEditType.SelectChannel)
        {
            this.chIndex = chIndex;
            this.createPos = createPos;
        }
    }

    public class MoveChannelParam : EditParam
    {
        public int chIndex;
        public Vector2 position;

        public MoveChannelParam() { }

        public MoveChannelParam(IPanelSync ownerPanel, eEditType editType, int chIndex, Vector2 position)
            : base(ownerPanel, eEditType.MoveChannel)
        {
            this.chIndex = chIndex;
            this.position = position;
        }
    }

    public class DeleteChannelParam : EditParam
    {
        public List<int> indices;

        public DeleteChannelParam() { }

        public DeleteChannelParam(IPanelSync ownerPanel, eEditType editType, List<int> indices)
            : base(ownerPanel, eEditType.DeleteChannel)
        {
            this.indices = indices;
        }
    }

    public class SelectChannelParm : EditParam
    {
        public List<int> indices;

        public SelectChannelParm() { }

        public SelectChannelParm(IPanelSync ownerPanel, eEditType editType, List<int> indices)
            : base(ownerPanel, eEditType.SelectChannel)
        {
            this.indices = indices;
        }
    }
}

