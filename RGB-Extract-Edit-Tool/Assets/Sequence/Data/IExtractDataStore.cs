using System.Collections.Generic;
using UnityEngine;


namespace DataExtract
{
    public interface IExtractDataStore
    {
        public class DatatStoreState
        {
            public List<IChannel> channels;
            public List<IGroup> groups;

            public DatatStoreState(List<IChannel> channels, List<IGroup> groups)
            {
                this.channels = new List<IChannel>();
                foreach (var channel in channels)
                {
                    this.channels.Add(channel.Clone());
                }

                this.groups = new List<IGroup>();
                foreach (var group in groups)
                {
                    this.groups.Add(group.Clone());
                }
            }

            public DatatStoreState()
            {
                channels = new List<IChannel>();
                groups = new List<IGroup>();
            }
        }

        List<IChannel> channels { get; set; }
        List<IGroup> groups { get; set; }

        public EditParam GetLastestEditParam();
        public DatatStoreState GetLastestStoreState();
        public DatatStoreState PopLastestStoreState();
        public DatatStoreState Undo();


        #region Channel

        public void CreateChannel(CreateChannelParam param);
        public void MoveChannel(MoveChannelParam param);
        public void MoveDeltaChannel(MoveDeltaChannelParam param);
        public void DeleteChannel(DeleteChannelParam param);

        #endregion

        #region Group

        public void MakeGroup(MakeGroupParam param);
        public void DeleteGroup(IGroup group);

        #endregion
    }
}