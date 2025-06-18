using UnityEngine;

namespace ScenarioVerification
{
    public class ScenarioDataUpdater
    {
        IScenarioDataStore scenarioDataStore;

        public ScenarioDataUpdater(IScenarioDataStore scenarioDataStore)
        {
            this.scenarioDataStore = scenarioDataStore;
        }

        public void Import(ImportResult importResult)
        {
            scenarioDataStore.Improt(importResult);
        }
    }
}