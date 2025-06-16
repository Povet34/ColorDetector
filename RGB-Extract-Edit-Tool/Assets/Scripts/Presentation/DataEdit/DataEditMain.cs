using UnityEngine;

namespace DataEdit
{
    public class DataEditMain : MonoBehaviour
    {
        public class EditDataInjection
        {
            public EditDataUpdater editDataUpdater;
            public EditDataReceiver editDataReceiver;
        }

        [SerializeField] DataEditTool dataEditTool;

        [SerializeField] HierarchyPanel hierarchyPanel;
        [SerializeField] ColorFlowViewPanel colorFlowViewPanel;

        DataExporter dataExporter;
        DataImporter dataImporter;

        EditDataUpdater editDataUpdater;
        EditDataReceiver editDataReceiver;

        private void Start()
        {
            IEditDataStore editDataStore = new EditDataStoreImp();

            LocalFileLoader_Excel localFileLoader_Excel = new();
            dataExporter = new(new ExportToExcel());
            dataImporter = new(new ImportFromExcel(), localFileLoader_Excel);
            
            editDataUpdater = new EditDataUpdater(editDataStore);
            editDataReceiver = new EditDataReceiver(editDataStore);

            EditDataInjection editDataInjection = new();
            editDataInjection.editDataReceiver = editDataReceiver;
            editDataInjection.editDataUpdater = editDataUpdater;


            dataEditTool.Init();

            hierarchyPanel.Init(editDataInjection);
            colorFlowViewPanel.Init(editDataInjection);
        }

        private void Awake()
        {
            Bus<LoadFromExcelEventArgs>.OnEvent += ImportSavedData;
        }

        private void OnDestroy()
        {
            Bus<LoadFromExcelEventArgs>.OnEvent -= ImportSavedData;
        }

        private void ImportSavedData(LoadFromExcelEventArgs args)
        {
            editDataUpdater.Import(dataImporter.Import());
        }
    }
}

