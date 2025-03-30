using DataExtract;
using NUnit.Framework;
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

    private IPanelSync panelSync;
    private RectTransform panelRt;
    Action<bool, SelectChannelParam> selectCallback;
    Action<bool, DeSelectChannelParam> clearSelectCallback;

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
    }

    public void PointerDown()
    {
        clearSelectCallback?.Invoke(false, new DeSelectChannelParam(panelSync));

        startPos = Input.mousePosition;
        selectionBox.gameObject.SetActive(true);
        isSelecting = true;
    }

    public void PointerUp()
    {
        var list = new List<int>();

        foreach (var channel in panelChannels)
        {
            if (null == channel)
                continue;

            if (IsWithinSelection(channel.position))
                list.Add(channel.channelIndex);
        }
        selectCallback.Invoke(false, new SelectChannelParam(panelSync, list));

        selectionBox.gameObject.SetActive(false);
        isSelecting = false;

        selectionBox.sizeDelta = Vector2.zero;
    }

    bool IsWithinSelection(Vector2 position)
    {
        Bounds bounds = new Bounds(TransformEx.GetRelativeAnchorPosition_Screen(panelRt, selectionBox.anchoredPosition), selectionBox.sizeDelta);
        return bounds.Contains(position);
    }


    public static RectAreaChannelSelection Create(
        RectTransform parentRt,
        IPanelSync panelSync,
        List<IPanelChannel> channels, 
        Action<bool, SelectChannelParam> addCurChannelList,
        Action<bool, DeSelectChannelParam> clearSelectCallback
        )
    {
        var go = new GameObject("AreaSelection");
        var selection = go.AddComponent<RectAreaChannelSelection>();
        selection.transform.SetParent(parentRt);

        var imgGo = new GameObject("selectAreaBox");
        imgGo.transform.SetParent(selection.transform);

        var img = imgGo.AddComponent<Image>();
        img.color = new Color(1, 1, 1, 0.3f);

        selection.selectionBox = imgGo.GetComponent<RectTransform>();
        selection.selectionBox.gameObject.SetActive(false);

        selection.panelRt = parentRt;
        selection.panelChannels = channels;

        selection.selectCallback = addCurChannelList;
        selection.clearSelectCallback = clearSelectCallback;
        selection.panelSync = panelSync;

        return selection;
    }
}
