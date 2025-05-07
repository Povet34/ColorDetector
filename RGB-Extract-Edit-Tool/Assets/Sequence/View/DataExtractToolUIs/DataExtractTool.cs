using DataExtract;
using System;
using UnityEngine;
using UnityEngine.UI;

public class DataExtractTool : MonoBehaviour
{
    #region Injection

    LocalFileLoader_Video fileLoader_Video;
    LocalFileLoader_Excel fileLoader_Excel;

    ChannelUpdater channelUpdater;

    #endregion

    [SerializeField] Button loadVideoButton;
    [SerializeField] Button loadExcelButton;
    [SerializeField] Button playVideoButton;
    [SerializeField] Button ExtractVideoButton;


    public void Init(DataExtractMain.LoadInjection injection)
    {
        fileLoader_Video = injection.fileLoader_Video;
        fileLoader_Excel = injection.fileLoader_Excel;

        channelUpdater = injection.channelUpdater;
    }

    private void Awake()
    {
        loadVideoButton.onClick.AddListener(OnLoadVideoButtonClicked);
        loadExcelButton.onClick.AddListener(OnLoadExcelButtonClicked);
        playVideoButton.onClick.AddListener(OnPlayVideoButtonClicked);
        ExtractVideoButton.onClick.AddListener(OnExtractVideoButtonClicked);
    }


    private void OnExtractVideoButtonClicked()
    {
        throw new NotImplementedException();
    }

    private void OnPlayVideoButtonClicked()
    {
        throw new NotImplementedException();
    }

    private void OnLoadExcelButtonClicked()
    {
        fileLoader_Video.OpenFilePath();
    }

    private void OnLoadVideoButtonClicked()
    {
        throw new NotImplementedException();
    }
}
