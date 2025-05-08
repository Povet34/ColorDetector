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

        ChannelReceiver channelReceiver = new ChannelReceiver();
        channelReceiver.Init(extractDataStore);

        ChannelUpdater channelUpdater = new ChannelUpdater();
        channelUpdater.Init(extractDataStore);

        ChannelSyncer channelSyncer = new ChannelSyncer();
        channelSyncer.Init(new List<IPanelSync>() { videoViewPanel, hierarchyPanel });

        VideoDataUpdater videoDataUpdater = new VideoDataUpdater();
        videoDataUpdater.Init(loadDataStore);

        VideoDataReceiver videoDataReceiver = new VideoDataReceiver();
        videoDataReceiver.Init(loadDataStore);

        PanelInjection panelInjection = new PanelInjection();
        panelInjection.channelReceiver = channelReceiver;
        panelInjection.channelUpdater = channelUpdater;
        panelInjection.channelSyncer = channelSyncer;
        panelInjection.videoDataReceiver = videoDataReceiver;

        LoadInjection loadInjection = new LoadInjection();
        loadInjection.videoDataUpdater = videoDataUpdater;
        loadInjection.videoDataReceiver = videoDataReceiver;
        loadInjection.channelUpdater = channelUpdater;
        loadInjection.channelReceiver = channelReceiver;

        ExportInjection exportInjection = new ExportInjection();
        exportInjection.toExcelExportor = new ToExcelExportor();

        videoViewPanel.Init(panelInjection);
        hierarchyPanel.Init(panelInjection);
        
        dataExtractTool.Init(loadInjection, exportInjection);
    }
}
