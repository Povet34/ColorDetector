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
        Dictionary<int, List<Color32>> extractMap { get; set; }


        #region VideoData

        Vector2 videoResoultion { get; set; }

        #endregion

        DatatStoreState Undo();

        #region Channel

        void CreateChannel(CreateChannelParam param);
        void MoveChannel(MoveChannelParam param);
        void MoveDeltaChannel(MoveDeltaChannelParam param);
        void DeleteChannel(DeleteChannelParam param);

        #endregion

        #region Group

        void MakeGroup(MakeGroupParam param);
        int GetGroupCount();
        bool CanGroup(List<int> indices);
        void MoveDeltaGroup(MoveDeltaGroupParam param);
        void ChangeGroupSortDirection(ChangeGroupSortDirectionParam param);
        void ReleaseGroup(ReleaseGroupParam param);
        void UnGroupForFree(UnGroupForFreeParam param);

        #endregion


        #region Extract

        void StoreStart();
        void StoreExtractData(Texture2D texture);

        #endregion
    }
}