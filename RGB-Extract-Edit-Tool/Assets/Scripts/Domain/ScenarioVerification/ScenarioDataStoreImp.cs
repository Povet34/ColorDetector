using System.Collections.Generic;
namespace ScenarioVerification
{
    public class ScenarioDataStoreImp : IScenarioDataStore
    {
        public string path { get; set; }
        public Dictionary<SavedChannelKey, SavedChannelValue> originRecordData { get; set; }
        public Dictionary<SavedChannelKey, SavedChannelValue> modifiedData { get; set; }
        public void Improt(ImportResult importResult)
        {
            path = importResult.path;

            originRecordData = importResult.originData.recordData;
            modifiedData = importResult.additionalData.modifiedRecordData;
        }
    }
}