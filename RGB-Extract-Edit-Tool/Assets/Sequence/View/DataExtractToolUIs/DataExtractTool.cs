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

    private void OnExtractVideoButtonClicked()
    {
        channelUpdater.Extract(null);
    }

    private void OnPlayVideoButtonClicked()
    {
        videoPlayer.Play();
    }

    private void OnLoadExcelButtonClicked()
    {
        SetInteractable(false);

        videoDataUpdater.UpdateLoadedVideoData_ByExcel();
        PrepareAndViewVideo();
    }

    private void OnLoadVideoButtonClicked()
    {
        SetInteractable(false);

        videoDataUpdater.UpdateLoadedVideoData_ByVideo();
        PrepareAndViewVideo();
    }

    private void SetInteractable(bool canInteract)
    {
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
}
