using UnityEngine;

public class VideoDataUpdater : MonoBehaviour
{
    ILoadDataStore loadDataStore;
    LocalFileLoader_Video videoFileLoader = new LocalFileLoader_Video();
    LocalFileLoader_Excel excelFileLoader = new LocalFileLoader_Excel();

    public void Init(ILoadDataStore loadDataStore)
    {
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
