using UnityEngine;

namespace DataExtract
{
    public class ChannelUpdater
    {
        IExtractDataStore _dataStore;

        public ChannelUpdater(IExtractDataStore dataStore)
        {
            _dataStore = dataStore;
        }

        public void CreateChannel(CreateChannelParam param)
        {
            _dataStore.CreateChannel(param);
        }

        public void DeleteChannel(DeleteChannelParam param)
        {
            _dataStore.DeleteChannel(param);
        }

        public void MoveChannel(MoveChannelParam param)
        {
            _dataStore.MoveChannel(param);
        }
        public void MoveDeltaChannel(MoveDeltaChannelParam param)
        {
            _dataStore.MoveDeltaChannel(param);
        }

        public IExtractDataStore.DatatStoreState Undo()
        {
            return _dataStore.Undo();
        }

        public void SetCurrentVideoResoultion(Vector2 newResoultion)
        {
            _dataStore.videoResolution = newResoultion;
        }

        public void MakeGroup(MakeGroupParam param)
        {
            _dataStore.MakeGroup(param);
        }

        public void MoveDeltaGroup(MoveDeltaGroupParam param)
        {
            _dataStore.MoveDeltaGroup(param);
        }

        public void ChangeGroupSortDirection(ChangeGroupSortDirectionParam param)
        {
            _dataStore.ChangeGroupSortDirection(param);
        }

        public void ReleaseGroup(ReleaseGroupParam param)
        {
            _dataStore.ReleaseGroup(param);
        }

        public void UnGroupForFree(UnGroupForFreeParam param)
        {
            _dataStore.UnGroupForFree(param);
        }

        public void StoreExtractData(Texture2D texture)
        {
            _dataStore.StoreExtractData(texture);
        }

        public void StoreStart()
        {
            _dataStore.StoreStart();
        }
    }
}