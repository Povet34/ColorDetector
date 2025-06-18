using DataEdit;
using UnityEngine;
using UnityEngine.UI;

namespace ScenarioVerification
{
    public class ScenarioTool : MonoBehaviour
    {
        #region Injection

        ScenarioDataUpdater scenarioDataUpdater;
        DataImporter dataImporter;

        #endregion

        [SerializeField] Button loadExcelButton;
        [SerializeField] Button playOriginScenarioButton;
        [SerializeField] Button playModifiedScenarioButton;

        public void Init(ScenarioVerificationMain.DataControllInjection dataControllInjection, ScenarioVerificationMain.DataMigrationInjection dataMigrationInjection)
        {
            scenarioDataUpdater = dataControllInjection.scenarioDataUpdater;
            dataImporter = dataMigrationInjection.dataImporter;
        }

        public void Awake()
        {
            loadExcelButton.onClick.AddListener(OnLoadExcelClicked);
            playOriginScenarioButton.onClick.AddListener(OnPlayOriginScenarioClicked);
            playModifiedScenarioButton.onClick.AddListener(OnPlayModifiedScenarioClicked);
        }

        private void OnLoadExcelClicked()
        {
            scenarioDataUpdater.Import(dataImporter.Import());
            Bus<RefreshPanelArgs>.Raise(new RefreshPanelArgs());
        }

        private void OnPlayOriginScenarioClicked()
        {
            Bus<PlayScenarioArgs>.Raise(new PlayScenarioArgs(0));
        }

        private void OnPlayModifiedScenarioClicked()
        {
            Bus<PlayScenarioArgs>.Raise(new PlayScenarioArgs(1));
        }
    }
}