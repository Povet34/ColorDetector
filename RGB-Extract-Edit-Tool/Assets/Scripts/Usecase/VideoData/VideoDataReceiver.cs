using System;
using System.Collections.Generic;
using UnityEngine;

public class VideoDataReceiver
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

    public Vector2 GetVideoResolution()
    {
        return loadDataStore.videoResolution;
    }

    public RenderTexture GetVideoTexture()
    {
        return loadDataStore.videoTexture;
    }

    public void RegistUpdateVideoTexture(Action callback)
    {
        loadDataStore.onUpdateVideoTexture += callback;
    }

    public void UnregistUpdateVideoTexture(Action callback)
    {
        loadDataStore.onUpdateVideoTexture -= callback;
    }

    public int GetTotalFrame()
    {
        return (int)loadDataStore.totalFrame;
    }
}
