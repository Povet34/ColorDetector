using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DataExtract
{
    public class RectAreaChannelSelection : MonoBehaviour
    {
        [SerializeField] RectTransform selectionBox;
        private Vector2 startPos;
        private bool isSelecting;
        private List<IPanelChannel> channels;

        private IPanelSync panelSync;
        private RectTransform panelRt;
        Action<SelectChannelParam> selectCallback;
        Action<DeSelectChannelParam> clearSelectCallback;

        private void Update()
        {
            if (isSelecting)
                ResizeRect();
        }

        public void Init(
            RectTransform panelRt,
            IPanelSync panelSync,
            ref List<IPanelChannel> channels,
            Action<SelectChannelParam> selectCallback,
            Action<DeSelectChannelParam> clearSelectCallback)
        {
            this.panelRt = panelRt;
            this.channels = channels;
            this.selectCallback = selectCallback;
            this.clearSelectCallback = clearSelectCallback;
            this.panelSync = panelSync;
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

            foreach (var channel in channels)
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
    }
}
