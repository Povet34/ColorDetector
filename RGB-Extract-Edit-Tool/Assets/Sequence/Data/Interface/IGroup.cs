using System.Collections.Generic;
using UnityEngine;

public interface IGroup
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
        public string name = "NewGroup";
        public SortDirection dir;
        public List<IGroupable> groupables;
    }

    public string name { get; set; }
    public List<IGroupable> hasGroupables { get; set; }
    public SortDirection sortDirection { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    /// <param name="info"></param>
    void Create(InitInfo info);
}
