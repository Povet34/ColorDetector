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
            public ChannelSyncer channelSyncer;
            public VideoDataReceiver videoDataReceiver;
        }

        public class LoadInjection
        {
            public VideoDataUpdater videoDataUpdater;
            public VideoDataReceiver videoDataReceiver;

            public ChannelUpdater channelUpdater;
            public ChannelReceiver channelReceiver;
        }

        public class DataControllInjection
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

            LocalFileLoader_Excel localFileLoader_Excel = new LocalFileLoader_Excel();
            LocalFileLoader_Video localFileLoader_Video = new LocalFileLoader_Video();

            ChannelReceiver channelReceiver = new(extractDataStore);
            ChannelUpdater channelUpdater = new(extractDataStore);
            ChannelSyncer channelSyncer = new(new List<IPanelSync>() { videoViewPanel, hierarchyPanel });
            VideoDataUpdater videoDataUpdater = new(loadDataStore, localFileLoader_Video, localFileLoader_Excel);
            VideoDataReceiver videoDataReceiver = new(loadDataStore);

            PanelInjection panelInjection = new();
            panelInjection.channelReceiver = channelReceiver;
            panelInjection.channelUpdater = channelUpdater;
            panelInjection.channelSyncer = channelSyncer;
            panelInjection.videoDataReceiver = videoDataReceiver;

            LoadInjection loadInjection = new();
            loadInjection.videoDataUpdater = videoDataUpdater;
            loadInjection.videoDataReceiver = videoDataReceiver;
            loadInjection.channelUpdater = channelUpdater;
            loadInjection.channelReceiver = channelReceiver;

            DataControllInjection exportInjection = new();
            exportInjection.dataExporter = new(new ExportToExcel());
            exportInjection.dataImporter = new(new ImportFromExcel(), localFileLoader_Excel);

            videoViewPanel.Init(panelInjection);
            hierarchyPanel.Init(panelInjection);

            dataExtractTool.Init(panelInjection, loadInjection, exportInjection);
        }
    }
}
