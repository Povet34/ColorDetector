using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;

namespace DataExtract
{
    public class HierarchyChannel : MonoBehaviour, IPanelChannel
    {
        public class CreateParam
        {
            public int chIndex;
            public Vector2 createPos;

            public Action<int, Vector2> onMoveCallback;
        }

        public int channelIndex { get; set; }
        public Vector2 position { get; set; }
        public int parentGroupIndex { get; set; } = -1;
        public int groupInIndex { get; set; } = -1;
        public IPanelGroup parentGroup { get; set; }

        Action<int, Vector2> onMoveCallback;

        public GameObject GetObject() => gameObject;

        [SerializeField] TMP_Text channelText;
        [SerializeField] TMP_InputField inputX;
        [SerializeField] TMP_InputField inputY;
        [SerializeField] Image bgImage;
        [SerializeField] GameObject groupNavigation;

        bool isSelect;

        void Awake()
        {
            inputX.onEndEdit.AddListener(
                (text)=> 
                {
                    int x = int.Parse(inputX.text);
                    int y = int.Parse(inputY.text);

                    onMoveCallback?.Invoke(channelIndex, new Vector2(x, y));
                });

            inputY.onEndEdit.AddListener(
                (text) =>
                {
                    int x = int.Parse(inputX.text);
                    int y = int.Parse(inputY.text);

                    onMoveCallback?.Invoke(channelIndex, new Vector2(x, y));
                });
        }

        public void Init(CreateParam param)
        {
            channelIndex = param.chIndex;
            position = param.createPos;
            onMoveCallback = param.onMoveCallback;

            channelText.text = $"CH {channelIndex}";

            UpdatePos();
            Deselect();
        }

        private void UpdatePos()
        {
            inputX.text = ((int)position.x).ToString();
            inputY.text = ((int)position.y).ToString();
        }
        public void DestroyChannel()
        {
            Destroy(gameObject);
        }

        public void Select()
        {
            bgImage.color = Definitions.SelectColor;
            isSelect = true;
        }

        public void Deselect()
        {
            bgImage.color = Definitions.DeselectColor;
            isSelect = false;
        }

        public bool IsSelect()
        {
            return isSelect;
        }

        public void Move(Vector2 pos)
        {
            position = pos;
            UpdatePos();
        }

        public void MoveDelta(Vector2 deltaPos)
        {
            position += deltaPos;
            UpdatePos();
        }

        public void SetGroup(IPanelGroup parentGroup, int groupInIndex)
        {
            this.parentGroup = parentGroup;
            this.groupInIndex = groupInIndex;

            parentGroupIndex = parentGroup.groupIndex;

            bool hasGroup = null != parentGroup;

            groupNavigation.SetActive(hasGroup);
            bgImage.rectTransform.sizeDelta = hasGroup ? new Vector2(300, 50) : new Vector2(350, 50);

            channelText.text = (hasGroup ? groupInIndex : channelIndex).ToString();
        }

        public bool HasGroup()
        {
            return null != parentGroup;
        }

        public void SelectForChangeHierarchy()
        {
            bgImage.color = Definitions.ChangeSelectColor;
        }
    }
}