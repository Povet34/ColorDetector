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
        public Action onRenameGroupButton;
        public Action onAddChannelsToGroupButton;

        public MenuActions(Action onDeleteChannel, Action onMakeGroup, Action onReleaseGroup, Action onUngroupForFree, Action onRenameGroupButton, Action onAddChannelsToGroupButton)
        {
            this.onDeleteChannel = onDeleteChannel;
            this.onMakeGroup = onMakeGroup;
            this.onReleaseGroup = onReleaseGroup;
            this.onUngroupForFree = onUngroupForFree;
            this.onRenameGroupButton = onRenameGroupButton;
            this.onAddChannelsToGroupButton = onAddChannelsToGroupButton;
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
        // OnSelectChannels ���� ��ư �ݹ�
        DeleteChannelsButton.onClick.AddListener(() => menuActions.onDeleteChannel());
        MakeGroupButton.onClick.AddListener(() => menuActions.onMakeGroup   ());
        UngroupForFreeButton.onClick.AddListener(() => menuActions.onUngroupForFree());

        // OnSelectGroup ���� ��ư �ݹ�
        RenameGroupButton.onClick.AddListener(() => menuActions.onRenameGroupButton());
        ReleaseGroupButton.onClick.AddListener(() => menuActions.onReleaseGroup());

        // OnSelectGroup | OnSelectChannels ���� ��ư �ݹ�
        AddChannelsToGroupButton.onClick.AddListener(() => menuActions.onAddChannelsToGroupButton());
    }

    public void Show(bool isShow, ShowType showType = ShowType.OnNothing)
    {
        if (!isShow)
        {
            gameObject.SetActive(false);
            return;
        }

        // OnSelectChannels ���� ��ư Ȱ��ȭ
        DeleteChannelsButton.gameObject.SetActive((showType & ShowType.OnSelectChannels) != 0);
        MakeGroupButton.gameObject.SetActive((showType & ShowType.OnSelectChannels) != 0);
        UngroupForFreeButton.gameObject.SetActive((showType & ShowType.OnSelectChannels) != 0);

        // OnSelectGroup ���� ��ư Ȱ��ȭ
        RenameGroupButton.gameObject.SetActive((showType & ShowType.OnSelectGroup) != 0);
        ReleaseGroupButton.gameObject.SetActive((showType & ShowType.OnSelectGroup) != 0);

        // OnSelectGroup | OnSelectChannels ���� ��ư Ȱ��ȭ
        AddChannelsToGroupButton.gameObject.SetActive((showType & (ShowType.OnSelectGroup | ShowType.OnSelectChannels)) == (ShowType.OnSelectGroup | ShowType.OnSelectChannels));

        gameObject.SetActive(true);
    }

    public void SetPosition(Vector2 position)
    {
        rt.anchoredPosition = position;
    }
}
