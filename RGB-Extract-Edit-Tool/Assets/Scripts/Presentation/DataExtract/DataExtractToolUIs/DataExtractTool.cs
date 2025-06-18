using DataExtract;
using System;
using System.Collections;
using System.Collections.Generic;
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
    PanelSyncer panelSyncer;

    DataExporter dataExporter;
    DataImporter dataImporter;

    #endregion

    [SerializeField] Button loadVideoButton;
    [SerializeField] Button loadExcelButton;
    [SerializeField] Button playVideoButton;
    [SerializeField] Button extractVideoButton;
    [SerializeField] Button exportExcelButton;
    [SerializeField] Button loadChannelPositionButton;

    VideoPlayer videoPlayer;
    ExtractTextureChanger extractTextureChanger;

    public void Init(DataExtractMain.PanelInjection panelInjection, DataExtractMain.LoadInjection injection, DataExtractMain.DataMigrationInjection exportInjection)
    {
        panelSyncer = panelInjection.panelSyncer;

        videoDataReceiver = injection.videoDataReceiver;
        videoDataUpdater = injection.videoDataUpdater;
        channelUpdater = injection.channelUpdater;
        channelReceiver = injection.channelReceiver;

        dataExporter = exportInjection.dataExporter;
        dataImporter = exportInjection.dataImporter;

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
        loadChannelPositionButton.onClick.AddListener(LoadChannelPositionButtonClicked);
    }

    private void LoadChannelPositionButtonClicked()
    {
        channelUpdater.ResetAll(new ResetAllParam());

        var channelDatas = dataImporter.LoadChannelInfos();
        if (channelDatas != null && channelDatas.Count > 0)
        {
            // 1. ä�� ����
            var groupDict = new Dictionary<string, List<int>>();
            var groupSortDirDict = new Dictionary<string, int>();
            foreach (var channelKey in channelDatas)
            {
                // ä�� ����
                var createParam = new CreateChannelParam(null, channelKey.index, channelKey.position, false);
                channelUpdater.CreateChannel(createParam);

                // �׷� ���� ����
                if (!string.IsNullOrEmpty(channelKey.groupName))
                {
                    if (!groupDict.ContainsKey(channelKey.groupName))
                        groupDict[channelKey.groupName] = new List<int>();
                    groupDict[channelKey.groupName].Add(channelKey.index);

                    // �׷캰 sortDirection ����(���� ���� ���� �� ���)
                    if (!groupSortDirDict.ContainsKey(channelKey.groupName))
                        groupSortDirDict[channelKey.groupName] = channelKey.sortDirection;
                }
            }

            // 2. �׷� ���� �� �׷� �ε��� ������
            var groupNameToIndex = new Dictionary<string, int>();
            int groupIndex = 0;
            foreach (var kvp in groupDict)
            {
                // �׷� ����
                var makeGroupParam = new MakeGroupParam(null, groupIndex, kvp.Value, (IGroup.SortDirection)groupSortDirDict[kvp.Key], kvp.Key, false);
                channelUpdater.MakeGroup(makeGroupParam);

                // �׷� �ε��� ���� ����
                groupNameToIndex[kvp.Key] = groupIndex;

                // �׷� �� ä�� �ε��� ������ (inIndex)
                for (int i = 0; i < kvp.Value.Count; i++)
                {
                    var channel = channelReceiver.GetChannel(kvp.Value[i]);
                    if (channel != null && channel.individualInfo != null)
                    {
                        channel.individualInfo.inIndex = channelDatas.Find(x => x.index == kvp.Value[i]).groupInindex;
                    }
                }

                groupIndex++;
            }

            // 3. �׷�� SortDirection ������
            foreach (var kvp in groupSortDirDict)
            {
                if (groupNameToIndex.TryGetValue(kvp.Key, out int idx))
                {
                    var changeSortParam = new ChangeGroupSortDirectionParam(null, idx, (IGroup.SortDirection)kvp.Value, false);
                    channelUpdater.ChangeGroupSortDirection(changeSortParam);
                }
            }

            // 4. �׷�� �̸� ������
            // ���� �׷� �̸��� �ٽ� �ٲ�� �Ѵٸ� �Ʒ� �ڵ� ���
            foreach (var kvp in groupNameToIndex)
            {
                var renameParam = new RenameGroupParam(null, kvp.Value, kvp.Key, false);
                channelUpdater.RenameGroup(renameParam);
            }

            // Last. �г� ��������
            panelSyncer.Refresh(new RefreshParam());
        }
        else
        {
            DLogger.LogError("Failed to load channel position data.");
        }
    }

    private void ExportExcelButtonClicked()
    {
        var extractedData = channelReceiver.GetExtractData();
        if (extractedData != null)
        {
            var originData = new OriginData();
            var recordDic = new Dictionary<SavedChannelKey, SavedChannelValue>();

            foreach (var kvp in extractedData)
            {
                int channelIndex = kvp.Key;

                Vector2 position = Vector2.zero;
                string groupName = string.Empty;
                int groupInindex = -1;
                int groupSortDir = -1;

                var channelInfo = channelReceiver.GetChannel(channelIndex);
                if (channelInfo != null)
                {
                    position = channelInfo.position;
                    if (null != channelInfo.individualInfo)
                    {
                        groupName = channelInfo.individualInfo.parentGroup.name;
                        groupInindex = channelInfo.individualInfo.inIndex;
                        groupSortDir = (int)channelInfo.individualInfo.parentGroup.sortDirection;
                    }
                }

                var key = new SavedChannelKey
                {
                    index = channelIndex,
                    position = position,
                    groupName = groupName,
                    groupInindex = groupInindex,
                    sortDirection = groupSortDir
                };
                var value = new SavedChannelValue
                {
                    colors = kvp.Value
                };
                recordDic.Add(key, value);
            }

            var colorList = TestColorSheet.Colors;
            var colorSheetData = new Dictionary<int, Color32>();
            if (colorList != null)
            {
                for (int i = 0; i < colorList.Length; i++)
                {
                    colorSheetData[i] = colorList[i];
                }
            }

            originData.orderType = OrderType.Channel_Index;
            originData.recordData = recordDic;
            originData.colorSheetData = colorSheetData;

            dataExporter.ExportNew(originData, Definitions.SavePath(System.IO.Path.GetFileNameWithoutExtension(videoDataReceiver.GetVideoUrl())));
        }
    }

    private void OnExtractVideoButtonClicked()
    {
        StartCoroutine(ExtractBody());
    }

    private IEnumerator ExtractBody()
    {
        //SetInteractable(false);

        int totalFrames = videoDataReceiver.GetTotalFrame();
        RenderTexture videoTexture = videoDataReceiver.GetVideoTexture();

        if (totalFrames <= 0 || videoTexture == null)
        {
            DLogger.LogError("Invalid video data. Cannot extract frames.");
            yield break;
        }

        DLogger.Log($"Starting extraction for {totalFrames} frames.");

        channelUpdater.StoreStart();

        videoPlayer.Prepare();
        while (!videoPlayer.isPrepared)
        {
            yield return null; // �غ� �Ϸ�� ������ ���
        }

        videoPlayer.targetTexture = videoTexture;

        for (int currentFrame = 0; currentFrame < totalFrames; currentFrame++)
        {
            videoPlayer.frame = currentFrame;
            videoPlayer.Play();

            // �������� RenderTexture�� ����� ������ ���
            yield return new WaitForEndOfFrame();
            videoPlayer.Pause();

            yield return null; // �� ������ �߰� ���
            yield return null; // �� ������ �߰� ���
            yield return null; // �� ������ �߰� ���

            // RenderTexture�� Texture2D�� ��ȯ
            var texture2D = extractTextureChanger.ChangeTexture(videoTexture);

            if (texture2D == null)
            {
                DLogger.LogError($"Failed to extract frame {currentFrame}.");
                continue;
            }

            channelUpdater.StoreExtractData(texture2D);

            DLogger.Log($"Frame {currentFrame + 1}/{totalFrames} extracted.");
        }

        extractTextureChanger.ReleaseCache();
        DLogger.Log("Video extraction completed.");

        //SetInteractable(true);
    }

    private void OnPlayVideoButtonClicked()
    {
        videoPlayer.Play();
    }

    private void OnLoadExcelButtonClicked()
    {
        //SetInteractable(false);
        videoDataUpdater.UpdateLoadedVideoData_ByExcel();
        PrepareAndViewVideo();
    }

    private void OnLoadVideoButtonClicked()
    {
        //SetInteractable(false);
        videoDataUpdater.UpdateLoadedVideoData_ByVideo();
        PrepareAndViewVideo();
    }

    private void SetInteractable(bool canInteract)
    {
        loadVideoButton.interactable = canInteract;
        loadExcelButton.interactable = canInteract;
        extractVideoButton.interactable = canInteract;
        playVideoButton.interactable = canInteract;
        exportExcelButton.interactable = canInteract;
    }

    private void PrepareAndViewVideo()
    {
        videoPlayer.url = videoDataReceiver.GetVideoUrl();
        videoPlayer.Prepare();

        videoPlayer.prepareCompleted += (source) =>
        {
            channelUpdater.SetCurrentVideoResoultion(videoDataReceiver.GetVideoResolution());
            //SetInteractable(true);
            
            videoPlayer.frame = 1;
            videoPlayer.Pause();
        };
    }

    private void ChangeRenderTexture()
    {
        videoPlayer.targetTexture = videoDataReceiver.GetVideoTexture();
    }
}
