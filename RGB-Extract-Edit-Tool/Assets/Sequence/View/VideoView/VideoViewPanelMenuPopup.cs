using System;
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
        public Action onReleaseGroup;

        public MenuActions(Action<Vector2> onCreateChannel, Action<Vector2> onCreateSegment, Action onDeleteChannel, Action onMakeGroup, Action onReleaseGroup)
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
        ReleaseGroupButton.onClick.AddListener(() =>    { menuActions.onReleaseGroup(); });

        //Show
        CreateChannelButton.onClick.AddListener(() =>   { Show(false); });
        CreateSegmentButton.onClick.AddListener(() =>   { Show(false); });
        DeleteChannelButton.onClick.AddListener(() =>   { Show(false); });
        MakeGroupButton.onClick.AddListener(() =>       { Show(false); });
        ReleaseGroupButton.onClick.AddListener(() =>    { Show(false); });
    }

    public void Show(bool isShow)
    {
        gameObject.SetActive(isShow);
    }

    public void SetPosition(Vector2 position)
    {
        rt.anchoredPosition = position;
    }
}
