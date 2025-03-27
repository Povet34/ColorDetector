using UnityEngine;
using UnityEngine.UI;

namespace DataExtract
{
    public class VideoViewChannel : MonoBehaviour, IPanelChannel
    {
        public int channelIndex { get; set; }
        public Vector2 position { get; set; }

        RectTransform rt;
        Image handleImage;

        void Awake()
        {
            rt = GetComponent<RectTransform>();
            handleImage = GetComponent<Image>();
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

            Deselect();
        }

        public void Move(MoveChannelParam param)
        {
            throw new System.NotImplementedException();
        }

        public void Select()
        {
            handleImage.color = new Color32(200, 50, 50, 255);
        }

        public void Deselect()
        {
            handleImage.color = new Color32(255, 255, 255, 255);
        }
    }
}