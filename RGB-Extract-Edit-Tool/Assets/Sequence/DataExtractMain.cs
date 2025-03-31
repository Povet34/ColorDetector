using UnityEngine;
using System.Collections.Generic;
using System;


namespace DataExtract
{
    public class DataExtractMain : MonoBehaviour
    {
        [SerializeField] HierarchyPanel hierarchyPanel;
        [SerializeField] VideoViewPanel videoViewPanel;

        private void Start()
        {
            IExtractDataStore extractDataStore = new ExtractDataStoreImp();

            ChannelReceiver channelReceiver = new ChannelReceiver();
            channelReceiver.Init(extractDataStore);

            ChannelUpdater channelUpdater = new ChannelUpdater();
            channelUpdater.Init(extractDataStore);

            ChannelSyncer channelSyncer = new ChannelSyncer();
            channelSyncer.Init(new List<IPanelSync>() { videoViewPanel, hierarchyPanel });

            videoViewPanel.Init(channelUpdater, channelReceiver, channelSyncer);
            hierarchyPanel.Init(channelUpdater, channelReceiver, channelSyncer);
        }
    }
}