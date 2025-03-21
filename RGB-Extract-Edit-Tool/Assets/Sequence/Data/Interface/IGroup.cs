using System.Collections.Generic;
using UnityEngine;

public interface IGroup
{
    public enum SortDirection //어떤 방향으로 채널들을 재정렬할지 정하는 key
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
    /// 생성
    /// </summary>
    /// <param name="info"></param>
    void Create(InitInfo info);
}
