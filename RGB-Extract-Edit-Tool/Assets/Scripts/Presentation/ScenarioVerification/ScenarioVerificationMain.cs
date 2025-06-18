using UnityEngine;

namespace ScenarioVerification
{
    public class ScenarioVerificationMain : MonoBehaviour
    {
        public class DataControllInjection
        {
            public ScenarioDataReceiver scenarioDataReceiver;
            public ScenarioDataUpdater scenarioDataUpdater;
        }

        public class DataMigrationInjection
        {
            public DataExporter dataExporter;
            public DataImporter dataImporter;
        }

        [SerializeField] ScenarioViewPanel scenarioViewPanel;
        [SerializeField] ScenarioTool scenarioTool;

        private void Start()
        {
            IScenarioDataStore scenarioDataStore = new ScenarioDataStoreImp();

            ScenarioDataReceiver scenarioDataReceiver = new(scenarioDataStore);
            ScenarioDataUpdater scenarioDataUpdater = new(scenarioDataStore);

            DataImporter dataImporter = new(new ImportFromExcel(), new LocalFileLoader_Excel());

            DataControllInjection dataControllInjection = new();
            dataControllInjection.scenarioDataReceiver = scenarioDataReceiver;
            dataControllInjection.scenarioDataUpdater = scenarioDataUpdater;

            DataMigrationInjection dataMigrationInjection = new();
            dataMigrationInjection.dataImporter = dataImporter;

            scenarioTool.Init(dataControllInjection, dataMigrationInjection);
            scenarioViewPanel.Init(dataControllInjection);
        }
    }
}
