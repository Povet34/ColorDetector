using DataExtract;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class DataExtractTool : MonoBehaviour
{
    #region Injection

    VideoDataReceiver videoDataReceiver;
    VideoDataUpdater videoDataUpdater;
    ChannelUpdater channelUpdater;

    #endregion

    [SerializeField] Button loadVideoButton;
    [SerializeField] Button loadExcelButton;
    [SerializeField] Button playVideoButton;
    [SerializeField] Button extractVideoButton;

    VideoPlayer videoPlayer;

    public void Init(DataExtractMain.LoadInjection injection)
    {
        videoDataReceiver = injection.videoDataReceiver;
        videoDataUpdater = injection.videoDataUpdater;
        channelUpdater = injection.channelUpdater;
    }

    private void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();

        loadVideoButton.onClick.AddListener(OnLoadVideoButtonClicked);
        loadExcelButton.onClick.AddListener(OnLoadExcelButtonClicked);
        playVideoButton.onClick.AddListener(OnPlayVideoButtonClicked);
        extractVideoButton.onClick.AddListener(OnExtractVideoButtonClicked);
    }

    private void OnEnable()
    {
        ClearRenderTexture(videoPlayer.targetTexture);
    }

    private void OnExtractVideoButtonClicked()
    {
        channelUpdater.Extract(videoPlayer.targetTexture);
    }

    private void OnPlayVideoButtonClicked()
    {
        videoPlayer.Play();
    }

    private void OnLoadExcelButtonClicked()
    {
        videoDataUpdater.UpdateLoadedVideoData_ByExcel();
        PrepareAndViewVideo();
    }

    private void OnLoadVideoButtonClicked()
    {
        videoDataUpdater.UpdateLoadedVideoData_ByVideo();
        PrepareAndViewVideo();
    }

    private void SetInteractable(bool canInteract)
    {
        var rt = videoPlayer.targetTexture;
        ResizeRenderTexture(ref rt, videoDataReceiver.GetVideoResolution());

        extractVideoButton.interactable = canInteract;
        playVideoButton.interactable = canInteract;
    }

    private void PrepareAndViewVideo()
    {
        videoPlayer.url = videoDataReceiver.GetVideoUrl();
        videoPlayer.Prepare();

        videoPlayer.prepareCompleted += (source) =>
        {
            SetInteractable(true);
            
            videoPlayer.frame = 0;
            videoPlayer.Pause();
        };
    }

    #region RenderTexture

    void ClearRenderTexture(RenderTexture rt)
    {
        RenderTexture currentRT = RenderTexture.active;
        RenderTexture.active = rt;

        GL.Clear(true, true, Color.black);

        RenderTexture.active = currentRT;
    }

    void ResizeRenderTexture(ref RenderTexture renderTexture, Vector2 newResolution)
    {
        if (newResolution == Vector2.zero)
        {
            Debug.LogError("New resolution is invalid. Cannot resize RenderTexture.");
            return;
        }

        int newWidth = (int)newResolution.x;
        int newHeight = (int)newResolution.y;

        // 기존 RenderTexture가 존재하면 해제
        if (renderTexture != null)
        {
            if (renderTexture.width == newWidth && renderTexture.height == newHeight)
            {
                Debug.Log("RenderTexture already matches the desired resolution. No resizing needed.");
                return;
            }

            renderTexture.Release();
            Debug.Log("Existing RenderTexture released.");
        }

        // 새로운 크기로 RenderTexture 생성
        renderTexture = new RenderTexture(newWidth, newHeight, 0, RenderTextureFormat.Default);
        renderTexture.Create();

        Debug.Log($"RenderTexture resized to: {newWidth}x{newHeight}");
    }

    #endregion
}
