using System.Collections.Generic;
using UnityEngine;

namespace ScenarioVerification
{
    public interface IScenarioDataStore
    {
        string path { get; set; }
        Dictionary<SavedChannelKey, SavedChannelValue> originRecordData { get; set; }
        Dictionary<SavedChannelKey, SavedChannelValue> modifiedData { get; set; }
        void Improt(ImportResult importResult);
    }
}

