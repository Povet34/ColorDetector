using UnityEngine;

public class DataExtractMain : MonoBehaviour
{
    [SerializeField] VideoViewPanel videoViewPanel;
    [SerializeField] HierarchyPanel hierarchyPanel;

    IChannelDataStore channelDataStore;

    private void Start()
    {
        channelDataStore = new ChannelDataStoreImp();

        IChannelGetter channelGetter = new ChannelGetterImp();
        channelGetter.Init(channelDataStore);

        IChannelUpdater channelUpdater = new ChannelUpdaterImp();
        channelUpdater.Init(channelDataStore);

        videoViewPanel.Init(channelDataStore, channelUpdater, channelGetter);
        hierarchyPanel.Init(channelDataStore, channelUpdater, channelGetter);
    }
}
