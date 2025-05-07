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
    [SerializeField] Button ExtractVideoButton;

    VideoPlayer videoPlayer;


    public void Init(DataExtractMain.LoadInjection injection)
    {

        channelUpdater = injection.channelUpdater;
    }

    private void Awake()
    {
        videoPlayer.GetComponent<VideoPlayer>();

        loadVideoButton.onClick.AddListener(OnLoadVideoButtonClicked);
        loadExcelButton.onClick.AddListener(OnLoadExcelButtonClicked);
        playVideoButton.onClick.AddListener(OnPlayVideoButtonClicked);
        ExtractVideoButton.onClick.AddListener(OnExtractVideoButtonClicked);
    }

    private void OnExtractVideoButtonClicked()
    {
        channelUpdater.Extract(null);
    }

    private void OnPlayVideoButtonClicked()
    {
        videoPlayer.url = fileLoader_Video.GetPath();
    }

    private void OnLoadExcelButtonClicked()
    {
        fileLoader_Excel.OpenFilePath();
    }

    private void OnLoadVideoButtonClicked()
    {
        fileLoader_Video.OpenFilePath();
    }
}
