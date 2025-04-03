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

        EditParam GetLastestEditParam();
        DatatStoreState GetLastestStoreState();
        DatatStoreState PopLastestStoreState();
        DatatStoreState Undo();

        #region Channel

        void CreateChannel(CreateChannelParam param);
        void MoveChannel(MoveChannelParam param);
        void MoveDeltaChannel(MoveDeltaChannelParam param);
        void DeleteChannel(DeleteChannelParam param);

        #endregion

        #region Group

        void MakeGroup(MakeGroupParam param);
        void DeleteGroup(IGroup group);
        int GetGroupCount();

        #endregion
    }
}