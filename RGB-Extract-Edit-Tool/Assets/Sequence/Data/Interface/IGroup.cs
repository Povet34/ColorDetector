using DataExtract;
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

    public int groupIndex { get; set; }
    public string name { get; set; }
    public List<int> hasChannels { get; set; }
    public SortDirection sortDirection { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    /// <param name="info"></param>
    void Create(MakeGroupParam param);
    void AddNewGroupable(int newTarget);
    void RemoveGroupable(int removeTarget);

    IGroup Clone();
}
