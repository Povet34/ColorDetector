using DataExtract;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VideoViewGroup : MonoBehaviour, IPanelGroup
{
    [SerializeField] Image bgImage;

    string groupName;
    public List<int> channelIndices { get; set; }
    public int groupIndex { get; set; }

    public void Deselect()
    {
        throw new System.NotImplementedException();
    }

    public GameObject GetObject()
    {
        return gameObject;
    }

    public void Init(MakeGroupParam param)
    {
        groupName = param.name;
        channelIndices = param.channelIndices;
    }

    public void Select()
    {
        throw new System.NotImplementedException();
    }
}
