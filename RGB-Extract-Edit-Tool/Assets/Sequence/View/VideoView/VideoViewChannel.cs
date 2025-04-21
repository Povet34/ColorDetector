using UnityEngine;
using UnityEngine.UI;

namespace DataExtract
{
    public class VideoViewChannel : MonoBehaviour, IPanelChannel
    {
        public int channelIndex { get; set; }
        public Vector2 position { get; set; }
        public int parentGroupIndex { get; set; } = -1;
        public int groupInIndex { get; set; } = -1;
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

        public void Init(CreateChannelParam param)
        {
            channelIndex = param.chIndex;
            position = param.createPos;

            rt.anchoredPosition = position;

            Deselect();
        }

        public void Select()
        {
            handleImage.color = Definitions.SelectColor;
            isSelect = true;
        }

        public void Deselect()
        {
            handleImage.color = Definitions.DeselectColor;
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
            this.parentGroup = parentGroup;
            this.groupInIndex = groupInIndex;

            parentGroupIndex = parentGroup.groupIndex;
        }

        public bool HasGroup()
        {
            return null != parentGroup;
        }

        public void SelectForChangeHierarchy()
        {
            handleImage.color = Definitions.ChangeSelectColor;
        }
    }
}