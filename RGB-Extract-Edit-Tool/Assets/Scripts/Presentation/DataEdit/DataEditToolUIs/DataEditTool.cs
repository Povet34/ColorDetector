using UnityEngine;
using UnityEngine.UI;
using static DataEdit.DataEditMain;
using static UnityEngine.Rendering.GPUSort;

namespace DataEdit
{
    public class DataEditTool : MonoBehaviour
    {
        #region Injection

        DataExporter dataExporter;
        DataImporter dataImporter;

        EditDataUpdater editDataUpdater;
        EditDataReceiver editDataReceiver;

        #endregion

        [SerializeField] Button loadFromExcelButton_Inferred;
        [SerializeField] Button loadFromExcelButton_Raw;
        [SerializeField] Button exportToExcelButton_Raw;

        private void Awake()
        {
            loadFromExcelButton_Raw.onClick.AddListener(OnLoadFromExcelButtonClicked_Raw);
            loadFromExcelButton_Inferred.onClick.AddListener(OnLoadFromExcelButtonClicked_Inferred);
            exportToExcelButton_Raw.onClick.AddListener(OnExportToExcelButtonClicked_Raw);
        }

        public void Init(EditDataInjection dataInjection, DataMigrationInjection dataMigrationInjection)
        {
            dataExporter = dataMigrationInjection.dataExporter;
            dataImporter = dataMigrationInjection.dataImporter;

            editDataReceiver = dataInjection.editDataReceiver;
            editDataUpdater = dataInjection.editDataUpdater;
        }

        void OnDestroy()
        {
            loadFromExcelButton_Raw.onClick.RemoveListener(OnLoadFromExcelButtonClicked_Raw);
            loadFromExcelButton_Inferred.onClick.RemoveListener(OnLoadFromExcelButtonClicked_Inferred);
            exportToExcelButton_Raw.onClick.RemoveListener(OnExportToExcelButtonClicked_Raw);
        }

        private void OnLoadFromExcelButtonClicked_Inferred()
        {
            editDataUpdater.Import(dataImporter.Import());
            Bus<RefreshPanelArgs>.Raise(new RefreshPanelArgs(0));
        }

        private void OnLoadFromExcelButtonClicked_Raw()
        {
            editDataUpdater.Import(dataImporter.Import());
            Bus<RefreshPanelArgs>.Raise(new RefreshPanelArgs(1));
        }
        private void OnExportToExcelButtonClicked_Raw()
        {
            AdditionalData additionalData = new AdditionalData()
            {
                modifiedRecordData = editDataReceiver.GetRecordedData(),
            };

            var recordedData = editDataReceiver.GetRecordedData();
            dataExporter.ExportAdd(additionalData, editDataReceiver.GetCurrentEditDataPath());
        }
    }
}