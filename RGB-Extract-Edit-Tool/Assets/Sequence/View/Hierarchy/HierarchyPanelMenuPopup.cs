using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HierarchyPanelMenuPopup : MonoBehaviour
{
    [Flags]
    public enum ShowType
    {
        OnNothing,
        OnSelectChannels,
        OnSelectGroup,
    }

    public struct MenuActions
    {
        public Action onDeleteChannel;
        public Action onMakeGroup;
        public Action onReleaseGroup;
        public Action onUngroupForFree;
        public Action onRenameGroup;
        public Action onAddChannelsToGroup;

        public MenuActions(Action onDeleteChannel, Action onMakeGroup, Action onReleaseGroup, Action onUngroupForFree, Action onRenameGroup, Action onAddChannelsToGroup)
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
        RenameGroupButton.onClick.AddListener(() => menuActions.onRenameGroup());
        ReleaseGroupButton.onClick.AddListener(() => menuActions.onReleaseGroup());

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

    public void Show(bool isShow, ShowType showType = ShowType.OnNothing)
    {
        if (!isShow)
        {
            gameObject.SetActive(false);
            return;
        }

        // OnSelectChannels 관련 버튼 활성화
        DeleteChannelsButton.interactable = (showType & ShowType.OnSelectChannels) != 0;
        MakeGroupButton.interactable = (showType & ShowType.OnSelectChannels) != 0;
        UngroupForFreeButton.interactable = (showType & ShowType.OnSelectChannels) != 0;

        // OnSelectGroup 관련 버튼 활성화
        RenameGroupButton.interactable = (showType & ShowType.OnSelectGroup) != 0;
        ReleaseGroupButton.interactable = (showType & ShowType.OnSelectGroup) != 0;

        // OnSelectGroup | OnSelectChannels 관련 버튼 활성화
        AddChannelsToGroupButton.interactable = (showType & (ShowType.OnSelectGroup | ShowType.OnSelectChannels)) == (ShowType.OnSelectGroup | ShowType.OnSelectChannels);

        gameObject.SetActive(true);
    }

    public void SetPosition(Vector2 position)
    {
        rt.anchoredPosition = position;
    }
}
