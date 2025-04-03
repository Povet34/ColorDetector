using UnityEngine;
using UnityEngine.UI;

namespace DataExtract
{
    public class VideoViewChannel : MonoBehaviour, IPanelChannel
    {
        public int channelIndex { get; set; }
        public Vector2 position { get; set; }
        public int parentGroupIndex { get; set; }
        public int groupInIndex { get; set; }
        public IPanelGroup parentGroup { get; set; }

        public GameObject GetObject() => gameObject;

        RectTransform rt;
        Image handleImage;
        bool isSelect;


        void Awake()
        {
            rt = GetComponent<RectTransform>();
            handleImage = GetComponent<Image>();
        }

        public void DestroyChannel()
        {
            Destroy(gameObject);    
        }

        void OnDestroy()
        {
            DLogger.Log_Blue(gameObject.name);
        }

        public void Init(CreateChannelParam param)
        {
            channelIndex = param.chIndex;
            position = param.createPos;

            rt.anchoredPosition = position;

            Deselect();
        }

        public void Select()
        {
            handleImage.color = new Color32(200, 50, 50, 255);
            isSelect = true;
        }

        public void Deselect()
        {
            handleImage.color = new Color32(255, 255, 255, 255);
            isSelect = false;
        }

        public bool IsSelect()
        {
            return isSelect;
        }

        public void MoveDelta(Vector2 deltaPos)
        {
            position += deltaPos;
            rt.anchoredPosition = position;
        }

        public void Move(Vector2 pos)
        {
            position = pos;
            rt.anchoredPosition = position;
        }

        public void SetGroup(IPanelGroup parentGroup, int groupInIndex)
        {
            throw new System.NotImplementedException();
        }

        public bool HasGroup()
        {
            return null != parentGroup;
        }
    }
}