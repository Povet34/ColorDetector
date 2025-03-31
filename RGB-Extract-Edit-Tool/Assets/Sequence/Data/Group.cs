using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using static IGroup;

public class Group : IGroup
{
    public string name { get; set; }
    public List<IChannel> hasChannels { get; set; }
    public SortDirection sortDirection { get; set; }

    public void Create(InitInfo info)
    {
        hasChannels = info.groupables;
        name = info.name;
        sortDirection = info.dir;
    }

    public IGroup Clone()
    {
        var clonedGroup = new Group
        {
            name = this.name,
            sortDirection = this.sortDirection,
            hasChannels = new List<IChannel>()
        };

        foreach (var ch in this.hasChannels)
        {
            clonedGroup.hasChannels.Add(ch.Clone());
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

    public void AddNewGroupable(IChannel newTarget)
    {
        hasChannels.Add(newTarget);
    }

    public void RemoveGroupable(IChannel removeTarget)
    {
        hasChannels.Remove(removeTarget);
    }
}