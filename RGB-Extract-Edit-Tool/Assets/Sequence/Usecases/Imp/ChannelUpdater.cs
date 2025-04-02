using UnityEngine;

namespace DataExtract
{
    public class ChannelUpdater
    {
        IExtractDataStore _dataStore;
        public void Init(IExtractDataStore dataStore)
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

        public void MakeGroup(MakeGroupParam param)
        {
            _dataStore.MakeGroup(param);
        }
    }
}