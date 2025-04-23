using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VideoViewPanelMenuPopup : MonoBehaviour
{
    public struct MenuActions
    {
        public Action<Vector2> onCreateChannel;
        public Action<Vector2> onCreateSegment;
        public Action onDeleteChannel;
        public Action onMakeGroup;
        public Action<int> onReleaseGroup;

        public MenuActions(Action<Vector2> onCreateChannel, Action<Vector2> onCreateSegment, Action onDeleteChannel, Action onMakeGroup, Action<int> onReleaseGroup)
        {
            this.onCreateChannel = onCreateChannel;
            this.onCreateSegment = onCreateSegment;
            this.onDeleteChannel = onDeleteChannel;
            this.onMakeGroup = onMakeGroup;
            this.onReleaseGroup = onReleaseGroup;
        }
    }

    [SerializeField] Button CreateChannelButton;
    [SerializeField] Button CreateSegmentButton;
    [SerializeField] Button DeleteChannelButton;
    [SerializeField] Button MakeGroupButton;
    [SerializeField] Button ReleaseGroupButton;

    RectTransform rt;

    int cachedGroupIndex = -1;
    List<int> cachedSelectChannels = null;

    private void Awake()
    {
        rt = GetComponent<RectTransform>();
    }

    public void Init(MenuActions menuActions)
    {
        CreateChannelButton.onClick.AddListener(() =>   { menuActions.onCreateChannel(rt.anchoredPosition); });
        CreateSegmentButton.onClick.AddListener(() =>   { menuActions.onCreateSegment(rt.anchoredPosition); });
        DeleteChannelButton.onClick.AddListener(() =>   { menuActions.onDeleteChannel(); });
        MakeGroupButton.onClick.AddListener(() =>       { menuActions.onMakeGroup(); });
        ReleaseGroupButton.onClick.AddListener(() =>    { menuActions.onReleaseGroup(cachedGroupIndex); });

        //Show
        CreateChannelButton.onClick.AddListener(() =>   { Show(false); });
        CreateSegmentButton.onClick.AddListener(() =>   { Show(false); });
        DeleteChannelButton.onClick.AddListener(() =>   { Show(false); });
        MakeGroupButton.onClick.AddListener(() =>       { Show(false); });
        ReleaseGroupButton.onClick.AddListener(() =>    { Show(false); });
    }

    public void Show(bool isShow, int groupIndex = -1, List<int> selectChannels = null)
    {
        cachedGroupIndex = groupIndex;
        cachedSelectChannels = selectChannels;
        
        if (!isShow)
        {
            gameObject.SetActive(false);
            return;
        }

        bool isExistGroup = groupIndex != -1;
        bool isExistSelectChannels = selectChannels != null && selectChannels.Count > 0;

        MakeGroupButton.interactable = isExistSelectChannels && !isExistGroup;

        ReleaseGroupButton.interactable = isExistGroup;

        gameObject.SetActive(true);
    }

    public void SetPosition(Vector2 position)
    {
        rt.anchoredPosition = position;
    }
}
