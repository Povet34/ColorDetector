using DataExtract;
using System;
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
        public Action<IGroup.SortDirection> onSort;
    }

    string groupName { get; set; }
    int groupIndex { get; set; }
    IGroup.SortDirection sortDirection { get; set; }
    List<IPanelChannel> hasChannels { get; set; }
    GameObject GetObject();
    void Init(Param param);
    void Select();
    void Deselect();
    void ChnageSortDirection();
}
