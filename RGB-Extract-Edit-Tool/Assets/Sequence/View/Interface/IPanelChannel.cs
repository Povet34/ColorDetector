using UnityEngine;

namespace DataExtract
{
    public interface IPanelChannel
    {
        int channelIndex { get; set; }
        Vector2 position { get; set; }

        void Init(CreateChannelParam param);
        void Move(MoveChannelParam param);
        void Destroy(DeleteChannelParam param);
    }
}