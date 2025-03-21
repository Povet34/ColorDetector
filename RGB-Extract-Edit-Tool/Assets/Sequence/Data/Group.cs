using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using static IGroup;

public class Group : IGroup
{
    public string name { get; set; }
    public List<IGroupable> hasGroupables { get; set; }
    public SortDirection sortDirection { get; set; }

    public void Create(InitInfo info)
    {
        hasGroupables = info.groupables;
        name = info.name;
        sortDirection = info.dir;
    }

    public void Reanme(string newName)
    {
        name = newName;
    }

    public int GetTotalGroupableCount()
    {
        return hasGroupables.Count;
    }

    public void SortGroupableOrder(SortDirection dir)
    {
    }

    public void AddNewGroupable(IGroupable newTarget)
    {
        hasGroupables.Add(newTarget);
    }

    public void RemoveGroupable(IGroupable removeTarget)
    {
        hasGroupables.Remove(removeTarget);
    }

}
