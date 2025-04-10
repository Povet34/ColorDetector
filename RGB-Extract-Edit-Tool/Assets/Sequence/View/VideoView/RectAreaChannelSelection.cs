using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DataExtract
{
    public class RectAreaChannelSelection : MonoBehaviour
    {
        private RectTransform selectionBox;
        private Vector2 startPos;
        private bool isSelecting;
        private List<IPanelChannel> panelChannels;

        private IPanelSync panelSync;
        private RectTransform panelRt;
        Action<SelectChannelParam> selectCallback;
        Action<DeSelectChannelParam> clearSelectCallback;

        private void Update()
        {
            if (isSelecting)
                ResizeRect();
        }

        public void ResizeRect()
        {
            Vector2 currentMousePos = Input.mousePosition;
            Vector2 size = currentMousePos - startPos;
            selectionBox.sizeDelta = new Vector2(Mathf.Abs(size.x), Mathf.Abs(size.y));
            selectionBox.anchoredPosition = startPos + size / 2f;
        }

        public void StartFindSelect()
        {
            selectionBox.sizeDelta = Vector2.zero;
            clearSelectCallback?.Invoke(new DeSelectChannelParam(panelSync));

            startPos = Input.mousePosition;
            selectionBox.gameObject.SetActive(true);
            isSelecting = true;
        }

        public bool IsRectActive()
        {
            return selectionBox.gameObject.activeSelf;
        }

        public List<int> EndFindSelect()
        {
            var list = new List<int>();

            foreach (var channel in panelChannels)
            {
                if (null == channel)
                    continue;

                if (IsIncludedBounds(channel.position))
                    list.Add(channel.channelIndex);
            }
            selectCallback.Invoke(new SelectChannelParam(panelSync, list));

            selectionBox.gameObject.SetActive(false);
            isSelecting = false;

            selectionBox.sizeDelta = Vector2.zero;

            return list;
        }


        bool IsIncludedBounds(Vector2 position)
        {
            Bounds bounds = new Bounds(TransformEx.GetRelativeAnchorPosition_Screen(panelRt, selectionBox.anchoredPosition), selectionBox.sizeDelta);
            return bounds.Contains(position);
        }


        public static RectAreaChannelSelection Create(
            RectTransform parentRt,
            IPanelSync panelSync,
            ref List<IPanelChannel> channels,
            Action<SelectChannelParam> addCurChannelList,
            Action<DeSelectChannelParam> clearSelectCallback
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

}
