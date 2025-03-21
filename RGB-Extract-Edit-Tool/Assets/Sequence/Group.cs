using NUnit.Framework;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Group : MonoBehaviour
{
    public enum SortDirection //어떤 방향으로 채널들을 재정렬할지 정하는 key
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
