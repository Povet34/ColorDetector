using System.Collections.Generic;
using UnityEngine;


namespace DataExtract
{
    public interface IExtractDataStore
    {
        List<IChannel> channels { get; set; }
        List<IGroup> groups { get; set; }

        #region Channel

        public void CreateChannel(CreateChannelParam param);
        public void MoveChannel(MoveChannelParam param);
        public void DeleteChannel(DeleteChannelParam param);
        public void SelectChannel(SelectChannelParam param);
        public void DeSelectChannel(DeSelectChannelParam param);
        public EditParam GetLastestEditParam();

        #endregion

        #region Group

        public void CreateGroup(IGroup.InitInfo info);
        public void DeleteGroup(IGroup group);

        #endregion
    }
}