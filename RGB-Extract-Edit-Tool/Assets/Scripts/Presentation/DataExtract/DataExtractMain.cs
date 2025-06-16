using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

namespace DataExtract
{
    public class DataExtractMain : MonoBehaviour
    {
        public class PanelInjection
        {
            public ChannelReceiver channelReceiver;
            public ChannelUpdater channelUpdater;
            public PanelSyncer panelSyncer;
            public VideoDataReceiver videoDataReceiver;
        }

        public class LoadInjection
        {
            public VideoDataUpdater videoDataUpdater;
            public VideoDataReceiver videoDataReceiver;

            public ChannelUpdater channelUpdater;
            public ChannelReceiver channelReceiver;
        }

        public class DataMigrationInjection
        {
            public DataExporter dataExporter;
            public DataImporter dataImporter;
        }

        [SerializeField] HierarchyPanel hierarchyPanel;
        [SerializeField] VideoViewPanel videoViewPanel;

        [SerializeField] DataExtractTool dataExtractTool;

        private void Start()
        {
            IExtractDataStore extractDataStore = new ExtractDataStoreImp();
            ILoadDataStore loadDataStore = new LoadDataStoreImpl();

            LocalFileLoader_Excel localFileLoader_Excel = new();
            LocalFileLoader_Video localFileLoader_Video = new();

            ChannelReceiver channelReceiver = new(extractDataStore);
            ChannelUpdater channelUpdater = new(extractDataStore);
            PanelSyncer channelSyncer = new(new List<IPanelSync>() { videoViewPanel, hierarchyPanel });
            VideoDataUpdater videoDataUpdater = new(loadDataStore, localFileLoader_Video, localFileLoader_Excel);
            VideoDataReceiver videoDataReceiver = new(loadDataStore);

            PanelInjection panelInjection = new();
            panelInjection.channelReceiver = channelReceiver;
            panelInjection.channelUpdater = channelUpdater;
            panelInjection.panelSyncer = channelSyncer;
            panelInjection.videoDataReceiver = videoDataReceiver;

            LoadInjection loadInjection = new();
            loadInjection.videoDataUpdater = videoDataUpdater;
            loadInjection.videoDataReceiver = videoDataReceiver;
            loadInjection.channelUpdater = channelUpdater;
            loadInjection.channelReceiver = channelReceiver;

            DataMigrationInjection migrationInjection = new();
            migrationInjection.dataExporter = new(new ExportToExcel());
            migrationInjection.dataImporter = new(new ImportFromExcel(), localFileLoader_Excel);

            videoViewPanel.Init(panelInjection);
            hierarchyPanel.Init(panelInjection);

            dataExtractTool.Init(panelInjection, loadInjection, migrationInjection);
        }
    }
}
