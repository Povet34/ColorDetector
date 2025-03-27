using System.Collections.Generic;
using UnityEngine;


namespace DataExtract
{
    public interface IChannel
    {
        public int channelIndex { get; set; }
        public Vector2 position { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="info"></param>
        void Create(CreateChannelParam praram);
    }
}