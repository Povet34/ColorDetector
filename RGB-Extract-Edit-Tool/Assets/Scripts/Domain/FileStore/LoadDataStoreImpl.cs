using System.IO;
using UnityEngine;
using System;

public class LoadDataStoreImpl : ILoadDataStore
{
    public string videoName { get; set; }
    public string videoUrl { get; set; }
    public float videoLength { get; set; }
    public Vector2 videoResolution { get; set; }
    public float videoFrameRate { get; set; }
    public ulong totalFrame { get; set; }
    public RenderTexture videoTexture { get; set; }
    public Action onUpdateVideoTexture { get; set; }

    public void UpdateLoadedVideoData(string path)
    {
        videoUrl = path;
        videoName = Path.GetFileName(path);

        // VideoPlayer�� ����Ͽ� ��Ÿ������ ����
        var videoPlayer = new GameObject("Tem pVideoPlayer").AddComponent<UnityEngine.Video.VideoPlayer>();
        videoPlayer.url = path;

        videoPlayer.Prepare();
        videoPlayer.prepareCompleted += (source) =>
        {
            videoLength = (float)videoPlayer.length;
            videoResolution = new Vector2(videoPlayer.width, videoPlayer.height);
            videoFrameRate = videoPlayer.frameRate;
            totalFrame = videoPlayer.frameCount;

            Debug.Log($"Video Loaded: {videoName}, Length: {videoLength}s, Resolution: {videoResolution}, FrameRate: {videoFrameRate}fps");

            GameObject.Destroy(videoPlayer.gameObject); // VideoPlayer ��ü ����

            CreateVideoRenderTarget();
        };
    }

    void CreateVideoRenderTarget()
    {
        // ���� RenderTexture�� �����ϸ� ����
        if (videoTexture != null)
        {
            videoTexture.Release();
            GameObject.Destroy(videoTexture);
            Debug.Log("Existing videoTexture released.");
        }

        // videoResolution�� �´� RenderTexture ����
        //int width = (int)videoResolution.x;
        //int height = (int)videoResolution.y;
        int width = (int)Definitions.VideoViewPanelSize.x;
        int height = (int)Definitions.VideoViewPanelSize.y;

        if (width <= 0 || height <= 0)
        {
            Debug.LogError("Invalid video resolution. Cannot create RenderTexture.");
            return;
        }

        videoTexture = new RenderTexture(width, height, 0, RenderTextureFormat.Default);
        videoTexture.Create();

        Debug.Log($"New videoTexture created with resolution: {videoTexture.width}x{videoTexture.height}");

        onUpdateVideoTexture?.Invoke();
    }
}
