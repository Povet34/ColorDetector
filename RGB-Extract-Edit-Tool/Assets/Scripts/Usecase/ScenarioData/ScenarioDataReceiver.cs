using System.Collections.Generic;
using System.Linq;

namespace ScenarioVerification
{
    public class ScenarioDataReceiver
    {
        IScenarioDataStore scenarioDataStore;

        public ScenarioDataReceiver(IScenarioDataStore scenarioDataStore)
        {
            this.scenarioDataStore = scenarioDataStore;
        }

        public List<SavedChannelKey> GetChannelDatas()
        {
            return scenarioDataStore.originRecordData.Keys.ToList();
        }

        public Dictionary<SavedChannelKey, SavedChannelValue> GetOriginDatas()
        {
            return scenarioDataStore.originRecordData;
        }

        public Dictionary<SavedChannelKey, SavedChannelValue> GetModifiedDatas()
        {
            return scenarioDataStore.modifiedData;
        }
    }
}