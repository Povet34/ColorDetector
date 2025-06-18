using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PaintArea : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public enum ViewUpdateMode { RegionPaint, AllPaint }
    public int totalCount;

    [SerializeField] ViewUpdateMode viewUpdateMode;
    [SerializeField] FrameNode frameNodePrefab;
    [SerializeField] GraphicRaycaster graphicRaycaster;
    [SerializeField] SummationView summationView;

    List<FrameNode> frameNodes = new List<FrameNode>();
    Brush brush;
    PointerEventData pointerEventData;
    EventSystem eventSystem;

    int startIndex;
    int endIndex;

    public void OnBeginDrag(PointerEventData eventData)
    {
        pointerEventData.position = eventData.position;
        List<RaycastResult> results = new List<RaycastResult>();
        graphicRaycaster.Raycast(pointerEventData, results);

        if (results.Count > 0)
        {
            startIndex = results[0].gameObject.GetComponent<FrameNode>()?.index ?? -1;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        pointerEventData.position = eventData.position;
        List<RaycastResult> results = new List<RaycastResult>();
        graphicRaycaster.Raycast(pointerEventData, results);

        foreach (RaycastResult result in results)
        {
            IPaintable paintable = result.gameObject.GetComponent<IPaintable>();
            if (paintable != null)
            {
                paintable.Paint(brush.GetColor());
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        pointerEventData.position = eventData.position;
        List<RaycastResult> results = new List<RaycastResult>();
        graphicRaycaster.Raycast(pointerEventData, results);

        if (results.Count > 0)
        {
            endIndex = results[0].gameObject.GetComponent<FrameNode>().index;
        }

        switch (viewUpdateMode)
        {
            case ViewUpdateMode.AllPaint:
                UpdateAllPaints();
                break;
            case ViewUpdateMode.RegionPaint:
                UpdateRegionPaints();
                break;
        }
    }

    public void EditStart(List<Color32> colors, Brush brush)
    {
        this.brush = brush;
        gameObject.SetActive(true);

        DestroyAll();

        totalCount = colors.Count;
        eventSystem = FindAnyObjectByType<EventSystem>();
        pointerEventData = new PointerEventData(eventSystem);

        SetInitColors(colors);

        for (int i = 0; i < colors.Count; i++)
        {
            FrameNode node = Instantiate(frameNodePrefab, transform);
            node.Init(new FrameNode.InitParam(i, colors[i]));

            frameNodes.Add(node);
        }

        //GridLayoutGroup layout = GetComponent<GridLayoutGroup>();

        //if (layout)
        //{
        //    float cellSizeX = layout.cellSize.x;
        //    RectOffset padding = layout.padding;

        //    GetComponent<RectTransform>().sizeDelta = new Vector2((cellSizeX + padding.left + padding.right) * totalCount, 315f);
        //}

       
        UpdateAllPaints();
    }

    public List<FrameNode> GetFrameNodes()
    {
        return frameNodes;
    }

    private void SetInitColors(List<Color32> colors)
    {
        for (int i = 0; i < colors.Count && i < frameNodes.Count; i++)
        {
            frameNodes[i].Paint(colors[i]);
        }
    }

    private void UpdateAllPaints()
    {
        SummationView.AllColor data = new SummationView.AllColor
        {
            colors = frameNodes.Select(node => node.Color).ToList()
        };

        summationView.UpdateView(data);
    }

    private void UpdateRegionPaints()
    {
        int start = Mathf.Min(startIndex, endIndex);
        int end = Mathf.Max(startIndex, endIndex);

        SummationView.RegionPaint data = new SummationView.RegionPaint
        {
            startIndex = start,
            endIndex = end,
            color = brush.GetColor()
        };

        summationView.UpdateView(data);
    }

    public void DestroyAll()
    {
        foreach (FrameNode node in frameNodes)
        {
            Destroy(node.gameObject);
        }
        frameNodes.Clear();
    }


    /// <summary>
    /// RawColor를 유지한 수정된 컬러집합 반환
    /// </summary>
    /// <returns></returns>
    public List<Color32> GetRawColors()
    {
        var result = new List<Color32>(frameNodes.Count);
        foreach (var node in frameNodes)
        {
            result.Add(node.Color);
        }
        return result;
    }
}