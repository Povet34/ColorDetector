using DataExtract;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class DataExtractTool : MonoBehaviour
{
    #region Injection

    VideoDataReceiver videoDataReceiver;
    VideoDataUpdater videoDataUpdater;
    ChannelUpdater channelUpdater;
    ChannelReceiver channelReceiver;

    #endregion

    [SerializeField] Button loadVideoButton;
    [SerializeField] Button loadExcelButton;
    [SerializeField] Button playVideoButton;
    [SerializeField] Button extractVideoButton;
    [SerializeField] Button exportExcelButton;

    VideoPlayer videoPlayer;
    ExtractTextureChanger extractTextureChanger;

    public void Init(DataExtractMain.LoadInjection injection)
    {
        videoDataReceiver = injection.videoDataReceiver;
        videoDataUpdater = injection.videoDataUpdater;
        channelUpdater = injection.channelUpdater;
        channelReceiver = injection.channelReceiver;

        extractTextureChanger = new ExtractTextureChanger();

        videoDataReceiver.RegistUpdateVideoTexture(ChangeRenderTexture);
    }

    private void OnDestroy()
    {
        videoDataReceiver.UnregistUpdateVideoTexture(ChangeRenderTexture);
    }

    private void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();

        loadVideoButton.onClick.AddListener(OnLoadVideoButtonClicked);
        loadExcelButton.onClick.AddListener(OnLoadExcelButtonClicked);
        playVideoButton.onClick.AddListener(OnPlayVideoButtonClicked);
        extractVideoButton.onClick.AddListener(OnExtractVideoButtonClicked);
        exportExcelButton.onClick.AddListener(ExportExcelButtonClicked);
    }

    private void ExportExcelButtonClicked()
    {
        var data = channelReceiver.GetExtractData();
        if(null != data)
        {
        }
    }

    private void OnExtractVideoButtonClicked()
    {
        StartCoroutine(ExtractBody());
    }

    private IEnumerator ExtractBody()
    {
        int count = 10;
        int temp = 0;

        // VideoDataReceiver로부터 데이터 가져오기
        int totalFrames = videoDataReceiver.GetTotalFrame();
        RenderTexture videoTexture = videoDataReceiver.GetVideoTexture();

        if (totalFrames <= 0 || videoTexture == null)
        {
            Debug.LogError("Invalid video data. Cannot extract frames.");
            yield break;
        }

        Debug.Log($"Starting extraction for {totalFrames} frames.");

        channelUpdater.StoreStart();

        // 0번 프레임부터 끝 프레임까지 반복
        for (int currentFrame = 0; currentFrame < totalFrames; currentFrame++)
        {
            // RenderTexture를 Texture2D로 변환
            var texture2D = extractTextureChanger.ChangeTexture(videoTexture);

            if (texture2D == null)
            {
                Debug.LogError($"Failed to extract frame {currentFrame}.");
                continue;
            }

            // ChannelUpdater의 Extract 메서드 호출
            channelUpdater.StoreExtractData(texture2D);

            temp++;
            if(temp == count)
            {
                yield return null;
                temp = 0;
            }

            Debug.Log($"Frame {currentFrame + 1}/{totalFrames} extracted.");
        }

        extractTextureChanger.ReleaseCache();
        Debug.Log("Video extraction completed.");
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
            
            videoPlayer.frame = 1;
            videoPlayer.Pause();
        };
    }

    private void ChangeRenderTexture()
    {
        videoPlayer.targetTexture = videoDataReceiver.GetVideoTexture();
    }
}
