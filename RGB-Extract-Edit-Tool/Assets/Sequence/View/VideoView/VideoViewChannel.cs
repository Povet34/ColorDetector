using UnityEngine;

namespace DataExtract
{
    public class VideoViewChannel : MonoBehaviour, IPanelChannel
    {
        public int channelIndex { get; set; }
        public Vector2 position { get; set; }

        RectTransform rt;

        void Awake()
        {
            rt = GetComponent<RectTransform>();
        }

        public void Destroy(DeleteChannelParam param)
        {
            throw new System.NotImplementedException();
        }

        public void Init(CreateChannelParam param)
        {
            channelIndex = param.chIndex;
            position = param.createPos;

            rt.anchoredPosition = position;
        }

        public void Move(MoveChannelParam param)
        {
            throw new System.NotImplementedException();
        }
    }
}