using UnityEngine;
using System.Collections.Generic;
using System;
using DataExtract;


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

    public class ExportInjection
    {
        public ToExcelExportor toExcelExportor;
    }

    [SerializeField] HierarchyPanel hierarchyPanel;
    [SerializeField] VideoViewPanel videoViewPanel;

    [SerializeField] DataExtractTool dataExtractTool;

    private void Start()
    {
        IExtractDataStore extractDataStore = new ExtractDataStoreImp();
        ILoadDataStore loadDataStore = new LoadDataStoreImpl();

        ChannelReceiver channelReceiver = new(extractDataStore);
        ChannelUpdater channelUpdater = new(extractDataStore);
        ChannelSyncer channelSyncer = new(new List<IPanelSync>() { videoViewPanel, hierarchyPanel });
        VideoDataUpdater videoDataUpdater = new(loadDataStore, new LocalFileLoader_Video(), new LocalFileLoader_Excel());
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

        ExportInjection exportInjection = new();
        exportInjection.toExcelExportor = new();

        videoViewPanel.Init(panelInjection);
        hierarchyPanel.Init(panelInjection);
        
        dataExtractTool.Init(loadInjection, exportInjection);
    }
}
