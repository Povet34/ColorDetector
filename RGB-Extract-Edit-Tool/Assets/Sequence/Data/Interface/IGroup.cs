using DataExtract;
using System.Collections.Generic;
using UnityEngine;

public interface IGroup
{
    public enum SortDirection //� �������� ä�ε��� ���������� ���ϴ� key
    {
        Left,
        Up,
        Right,
        Down
    }

    public int groupIndex { get; set; }
    public string name { get; set; }
    public List<IChannel> hasChannels { get; set; }
    public SortDirection sortDirection { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    /// <param name="info"></param>
    void Create(MakeGroupParam param, List<IChannel> channels);
    void AddNewGroupable(IChannel newTarget);
    void RemoveGroupable(IChannel removeTarget);

    IGroup Clone();
}
