using DataExtract;
using System.Collections.Generic;
using UnityEngine;

public interface IPanelGroup
{
    List<int> channelIndices { get; set; }
    void Init(MakeGroupParam param);
    void Select();
    void Deselect();
}
