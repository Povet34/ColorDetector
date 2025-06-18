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

        public class DataMigrationInjection
        {
            public DataExporter dataExporter;
            public DataImporter dataImporter;
        }

        [SerializeField] DataEditTool dataEditTool;
        [SerializeField] HierarchyPanel hierarchyPanel;
        [SerializeField] ColorFlowViewPanel colorFlowViewPanel;
        [SerializeField] ChannelEditPanel channelEditPanel;

        private void Start()
        {
            IEditDataStore editDataStore = new EditDataStoreImp();

            EditDataInjection editDataInjection = new();
            editDataInjection.editDataReceiver = new EditDataReceiver(editDataStore);
            editDataInjection.editDataUpdater = new EditDataUpdater(editDataStore);

            DataMigrationInjection dataMigrationInjection = new();
            dataMigrationInjection.dataImporter = new(new ImportFromExcel(), new LocalFileLoader_Excel());
            dataMigrationInjection.dataExporter = new(new ExportToExcel());

            hierarchyPanel.Init(editDataInjection);
            colorFlowViewPanel.Init(editDataInjection);
            channelEditPanel.Init(editDataInjection);
            dataEditTool.Init(editDataInjection, dataMigrationInjection);
        }
    }
}

