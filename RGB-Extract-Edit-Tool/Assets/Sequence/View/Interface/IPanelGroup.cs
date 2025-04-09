using DataExtract;
using System.Collections.Generic;
using UnityEngine;

public interface IPanelGroup
{
    public class Param
    {
        public List<IPanelChannel> hasChannels;
        public string name;
        public IGroup.SortDirection sortDirection;
        public int groupIndex;
    }

    string groupName { get; set; }
    int groupIndex { get; set; }
    List<IPanelChannel> hasChannels { get; set; }
    GameObject GetObject();
    void Init(Param param);
    void Select();
    void Deselect();
}
