using DataExtract;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RectAreaChannelSelection : MonoBehaviour
{
    private RectTransform selectionBox;
    private Vector2 startPos;
    private bool isSelecting;
    private List<IPanelChannel> panelChannels;
    private Action<IPanelChannel> addCurChannelList;
    private Action clearCurChannelList;

    private void Update()
    {
        if (isSelecting)
            Select();
    }

    public void Select()
    {
        Vector2 currentMousePos = Input.mousePosition;
        Vector2 size = currentMousePos - startPos;
        selectionBox.sizeDelta = new Vector2(Mathf.Abs(size.x), Mathf.Abs(size.y));
        selectionBox.anchoredPosition = startPos + size / 2f;

        clearCurChannelList?.Invoke();

        foreach (var channel in panelChannels)
        {
            if (null == channel)
                continue;

            if (IsWithinSelection(channel.position))
                addCurChannelList?.Invoke(channel);
        }
    }

    public void PointerDown()
    {
        startPos = Input.mousePosition;
        selectionBox.gameObject.SetActive(true);
        isSelecting = true;
    }

    public void Cancel()
    {
        selectionBox.gameObject.SetActive(false);
        isSelecting = false;
    }

    bool IsWithinSelection(Vector2 position)
    {
        Bounds bounds = new Bounds(selectionBox.position, selectionBox.sizeDelta);
        return bounds.Contains(position);
    }


    public static RectAreaChannelSelection Create(Transform parent, List<IPanelChannel> channels, Action<IPanelChannel> addCurChannelList, Action clearCurChannelList)
    {
        var go = new GameObject("AreaSelection");
        var selection = go.AddComponent<RectAreaChannelSelection>();
        selection.transform.SetParent(parent);

        var imgGo = new GameObject("selectAreaBox");
        imgGo.transform.SetParent(selection.transform);

        var img = imgGo.AddComponent<Image>();
        img.color = new Color(1, 1, 1, 0.3f);

        selection.selectionBox = imgGo.GetComponent<RectTransform>();
        selection.selectionBox.gameObject.SetActive(false);

        selection.panelChannels = channels;

        selection.addCurChannelList = addCurChannelList;
        selection.clearCurChannelList = clearCurChannelList;

        return selection;
    }
}
