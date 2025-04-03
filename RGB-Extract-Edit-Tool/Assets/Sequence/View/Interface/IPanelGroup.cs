using DataExtract;
using System.Collections.Generic;
using UnityEngine;

public interface IPanelGroup
{
    int groupIndex { get; set; }
    List<int> channelIndices { get; set; }
    GameObject GetObject();
    void Init(MakeGroupParam param);
    void Select();
    void Deselect();
}
