using System.Collections.Generic;
using UnityEngine;

public interface IChannel
{
    public class InitInfo
    {
        public int channelIndex;
        public Vector2 position;
    }

    public int channelIndex { get; set; }
    public Vector2 position { get; set; }

    /// <summary>
    /// »ý¼º
    /// </summary>
    /// <param name="info"></param>
    void Create(InitInfo info);
}
