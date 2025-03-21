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

    public class InitInfo
    {
        public int groupIndex;
        public string name;
        public SortDirection dir;
        public List<IGroupable> groupables;
    }


    public void CreateGroup(InitInfo info)
    {

    }

    public void Reanme(string newName)
    {
        name = newName;
    }
}
