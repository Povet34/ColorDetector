using DataExtract;
using System.Collections.Generic;
using UnityEngine;

public class VideoViewGroup : MonoBehaviour, IPanelGroup
{
    public List<int> channelIndices { get; set; }


    public void Deselect()
    {
        throw new System.NotImplementedException();
    }

    public GameObject GetObject()
    {
        throw new System.NotImplementedException();
    }

    public void Init(MakeGroupParam param)
    {
        channelIndices = param.channelIndices;
    }

    public void Select()
    {
        throw new System.NotImplementedException();
    }
}
