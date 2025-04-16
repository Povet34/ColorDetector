using DataExtract;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VideoViewGroup : MonoBehaviour, IPanelGroup
{
    [SerializeField] Image bgImage;

    public string groupName { get;set; }
    public List<IPanelChannel> hasChannels { get; set; }
    public int groupIndex { get; set; }

    public void Deselect()
    {
        bgImage.gameObject.SetActive(false);
    }

    public GameObject GetObject()
    {
        return gameObject;
    }

    public void Init(IPanelGroup.Param param)
    {
        DLogger.Log_Blue($"MakeGroup : {param.groupIndex}");

        groupName = param.name;
        hasChannels = param.hasChannels;
        groupIndex = param.groupIndex;
    }

    public void Select()
    {
        Vector2 minPos = Vector2.positiveInfinity;
        Vector2 maxPos = Vector2.negativeInfinity;

        foreach (var channel in hasChannels)
        {
            RectTransform channelRectTransform = channel.GetObject().GetComponent<RectTransform>();
            Vector2 channelPos = channelRectTransform.anchoredPosition;
            Vector2 channelSize = channelRectTransform.sizeDelta;

            minPos = Vector2.Min(minPos, channelPos - channelSize / 2);
            maxPos = Vector2.Max(maxPos, channelPos + channelSize / 2);
        }

        // �׷��� ũ��� ��ġ�� ����
        Vector2 groupSize = maxPos - minPos;
        Vector2 groupCenter = (minPos + maxPos) / 2;

        RectTransform groupRectTransform = GetComponent<RectTransform>();
        groupRectTransform.sizeDelta = groupSize;
        groupRectTransform.anchoredPosition = groupCenter;

        bgImage.gameObject.SetActive(true);

        // bgImage�� ũ��� ��ġ�� �׷쿡 �°� ����
        //RectTransform bgRectTransform = bgImage.GetComponent<RectTransform>();
        //bgRectTransform.sizeDelta = groupSize;
        //bgRectTransform.anchoredPosition = Vector2.zero;
    }
}
