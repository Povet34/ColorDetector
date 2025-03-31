using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace DataExtract
{
    public class HierarchyChannel : MonoBehaviour, IPanelChannel
    {
        public int channelIndex { get; set; }
        public Vector2 position { get; set; }

        public GameObject GetObject() => gameObject;

        [SerializeField] TMP_Text channelText;
        [SerializeField] TMP_InputField inputX;
        [SerializeField] TMP_InputField inputY;
        Image bgImage;

        bool isSelect;

        void Awake()
        {
            bgImage = GetComponent<Image>();
        }

        public void Init(CreateChannelParam param)
        {
            channelIndex = param.chIndex;
            position = param.createPos;

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
            bgImage.color = new Color32(200, 50, 50, 255);
            isSelect = true;
        }

        public void Deselect()
        {
            bgImage.color = new Color32(96, 96, 96, 255);
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
    }
}