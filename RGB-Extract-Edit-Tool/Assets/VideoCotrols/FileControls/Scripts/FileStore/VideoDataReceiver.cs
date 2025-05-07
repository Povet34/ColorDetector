using UnityEngine;

public class VideoDataReceiver : MonoBehaviour
{
    ILoadDataStore loadDataStore;

    public void Init(ILoadDataStore loadDataStore)
    {
        this.loadDataStore = loadDataStore;
    }

    public string GetVideoUrl()
    {
        return loadDataStore.videoUrl;
    }
}
