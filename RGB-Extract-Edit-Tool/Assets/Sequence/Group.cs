using NUnit.Framework;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Group : MonoBehaviour
{
    public enum SortDirection //� �������� ä�ε��� ���������� ���ϴ� key
    {
        Left,
        Right,
        Up,
        Down
    }

    string name;
    List<IGroupable> hasGroupables;
    SortDirection sortDirection;

    public class InitInfo
    {
        public int groupIndex;
        public string name = "NewGroup";
        public SortDirection dir;
        public List<IGroupable> groupables;
    }

    public void CreateGroup(InitInfo info)
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

    public void AddNew
}
