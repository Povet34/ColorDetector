using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using static IGroup;
using DataExtract;

public class Group : IGroup
{
    public string name { get; set; }
    public List<IChannel> hasChannels { get; set; }
    public SortDirection sortDirection { get; set; }
    public int groupIndex { get; set; }

    public void Create(MakeGroupParam param, List<IChannel> channels)
    {
        groupIndex = param.groupIndex;
        hasChannels = channels;
        name = param.name;
        sortDirection = param.sortDirection;
    }

    public IGroup Clone()
    {
        var clonedGroup = new Group
        {
            name = this.name,
            sortDirection = this.sortDirection,
            hasChannels = new List<IChannel>(),
            groupIndex = this.groupIndex
        };

        foreach (var ch in hasChannels)
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

    public void ReleaseGroup()
    {
        // 그룹에 속한 모든 채널 갱신
        foreach (var channel in hasChannels)
        {
            if (channel != null)
            {
                // 채널의 그룹 정보 제거
                channel.ExcludeGroup(); // ExcludeGroup 메서드 호출
            }
        }

        // 그룹의 채널 리스트 초기화
        hasChannels.Clear();
    }
}