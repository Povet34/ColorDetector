using UnityEngine;

public class VideoDataUpdater
{
    ILoadDataStore loadDataStore;
    LocalFileLoader_Video videoFileLoader;
    LocalFileLoader_Excel excelFileLoader;

    public VideoDataUpdater(ILoadDataStore loadDataStore, LocalFileLoader_Video videoFileLoader, LocalFileLoader_Excel excelFileLoader)
    {
        this.loadDataStore = loadDataStore;
        this.videoFileLoader = videoFileLoader;
        this.excelFileLoader = excelFileLoader;
    }

    public void UpdateLoadedVideoData_ByVideo()
    {
        string url = videoFileLoader.OpenFilePath();
        loadDataStore.UpdateLoadedVideoData(url);
    }

    public void UpdateLoadedVideoData_ByExcel()
    {
        string url = excelFileLoader.OpenFilePath();
        loadDataStore.UpdateLoadedVideoData(url);
    }
}
