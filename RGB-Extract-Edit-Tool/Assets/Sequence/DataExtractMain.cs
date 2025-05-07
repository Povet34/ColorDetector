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
    }

    public class LoadInjection
    {
        public LocalFileLoader_Video fileLoader_Video;
        public LocalFileLoader_Excel fileLoader_Excel;
        public ChannelUpdater channelUpdater;
    }

    [SerializeField] HierarchyPanel hierarchyPanel;
    [SerializeField] VideoViewPanel videoViewPanel;

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

        LocalFileLoader_Video fileLoader_Video = new LocalFileLoader_Video(loadDataStore);

        PanelInjection panelInjection = new PanelInjection();
        panelInjection.channelReceiver = channelReceiver;
        panelInjection.channelUpdater = channelUpdater;
        panelInjection.channelSyncer = channelSyncer;

        LoadInjection loadInjection = new LoadInjection();
        loadInjection.fileLoader_Video = fileLoader_Video;
        loadInjection.channelUpdater = channelUpdater;

        videoViewPanel.Init(panelInjection);
        hierarchyPanel.Init(panelInjection);
    }
}
