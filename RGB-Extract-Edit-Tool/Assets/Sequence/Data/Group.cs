using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using static IGroup;
using DataExtract;

public class Group : IGroup
{
    public string name { get; set; }
    public List<int> hasChannels { get; set; }
    public SortDirection sortDirection { get; set; }

    public void Create(MakeGroupParam param)
    {
        hasChannels = param.channelIndices;
        name = param.name;
        sortDirection = param.sortDirection;
    }

    public IGroup Clone()
    {
        var clonedGroup = new Group
        {
            name = this.name,
            sortDirection = this.sortDirection,
            hasChannels = new List<int>()
        };

        foreach (int index in hasChannels)
        {
            clonedGroup.hasChannels.Add(index);
        }

        return clonedGroup;
    }

    public void Reanme(string newName)
    {
        name = newName;
    }

    public int GetTotalGroupableCount()
    {
        return hasChannels.Count;
    }

    public void SortGroupableOrder(SortDirection dir)
    {
    }

    public void AddNewGroupable(int newTarget)
    {
        hasChannels.Add(newTarget);
    }

    public void RemoveGroupable(int removeTarget)
    {
        hasChannels.Remove(removeTarget);
    }
}