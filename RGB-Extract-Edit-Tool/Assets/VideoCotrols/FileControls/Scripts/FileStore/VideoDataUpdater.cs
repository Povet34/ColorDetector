using UnityEngine;

public class VideoDataUpdater
{
    ILoadDataStore loadDataStore;
    LocalFileLoader_Video videoFileLoader;
    LocalFileLoader_Excel excelFileLoader;

    public void Init(ILoadDataStore loadDataStore)
    {
        videoFileLoader = new LocalFileLoader_Video();
        excelFileLoader = new LocalFileLoader_Excel();

        this.loadDataStore = loadDataStore;
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
