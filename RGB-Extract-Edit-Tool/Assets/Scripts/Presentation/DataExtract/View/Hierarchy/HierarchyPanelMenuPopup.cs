using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HierarchyPanelMenuPopup : MonoBehaviour
{
    public struct MenuActions
    {
        public Action onDeleteChannel;
        public Action onMakeGroup;
        public Action<int> onReleaseGroup;
        public Action onUngroupForFree;
        public Action<int> onRenameGroup;
        public Action onAddChannelsToGroup;

        public MenuActions(Action onDeleteChannel, Action onMakeGroup, Action<int> onReleaseGroup, Action onUngroupForFree, Action<int> onRenameGroup, Action onAddChannelsToGroup)
        {
            this.onDeleteChannel = onDeleteChannel;
            this.onMakeGroup = onMakeGroup;
            this.onReleaseGroup = onReleaseGroup;
            this.onUngroupForFree = onUngroupForFree;
            this.onRenameGroup = onRenameGroup;
            this.onAddChannelsToGroup = onAddChannelsToGroup;
        }
    }


    [SerializeField] Button DeleteChannelsButton;
    [SerializeField] Button MakeGroupButton;
    [SerializeField] Button ReleaseGroupButton;

    [SerializeField] Button UngroupForFreeButton;
    [SerializeField] Button RenameGroupButton;
    [SerializeField] Button AddChannelsToGroupButton;

    RectTransform rt;

    int cachedGroupIndex = -1;
    List<int> cachedSelectChannels = null;

    private void Awake()
    {
        rt = GetComponent<RectTransform>();
    }

    public void Init(MenuActions menuActions)
    {
        // OnSelectChannels 관련 버튼 콜백
        DeleteChannelsButton.onClick.AddListener(() => menuActions.onDeleteChannel());
        MakeGroupButton.onClick.AddListener(() => menuActions.onMakeGroup());
        UngroupForFreeButton.onClick.AddListener(() => menuActions.onUngroupForFree());

        // OnSelectGroup 관련 버튼 콜백
        RenameGroupButton.onClick.AddListener(() => menuActions.onRenameGroup(cachedGroupIndex));
        ReleaseGroupButton.onClick.AddListener(() => menuActions.onReleaseGroup(cachedGroupIndex));

        // OnSelectGroup | OnSelectChannels 관련 버튼 콜백
        AddChannelsToGroupButton.onClick.AddListener(() => menuActions.onAddChannelsToGroup());

        //Show
        AddChannelsToGroupButton.onClick.AddListener(() => { Show(false); });
        ReleaseGroupButton.onClick.AddListener(() => { Show(false); });
        RenameGroupButton.onClick.AddListener(() => { Show(false); });
        UngroupForFreeButton.onClick.AddListener(() => { Show(false); });
        MakeGroupButton.onClick.AddListener(() => { Show(false); });
        DeleteChannelsButton.onClick.AddListener(() => { Show(false); });
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

        // OnSelectChannels 관련 버튼 활성화
        DeleteChannelsButton.interactable = isExistSelectChannels;
        MakeGroupButton.interactable = isExistSelectChannels && !isExistGroup;
        UngroupForFreeButton.interactable = isExistSelectChannels;

        // OnSelectGroup 관련 버튼 활성화
        RenameGroupButton.interactable = isExistGroup;
        ReleaseGroupButton.interactable = isExistGroup;

        // OnSelectGroup | OnSelectChannels 관련 버튼 활성화
        AddChannelsToGroupButton.interactable = isExistGroup && isExistSelectChannels;

        gameObject.SetActive(true);
    }

    public void SetPosition(Vector2 position)
    {
        rt.anchoredPosition = position;
    }
}
