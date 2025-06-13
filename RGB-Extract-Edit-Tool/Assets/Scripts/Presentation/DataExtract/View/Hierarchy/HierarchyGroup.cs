using TMPro;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using Unity.VisualScripting;

namespace DataExtract
{
    public class HierarchyGroup : MonoBehaviour, IPanelGroup
    {
        Vector3 Right = new Vector3(0, 0, 180);
        Vector3 Left = new Vector3(0, 0, 0);
        Vector3 Up = new Vector3(0, 0, 90);
        Vector3 Down = new Vector3(0, 0, 270);

        [SerializeField] TMP_Text groupNameText;
        [SerializeField] Image bgImage;
        [SerializeField] Button sortDirectionButton;
        bool isSelect = false;

        public List<IPanelChannel> hasChannels { get; set; }
        public int groupIndex { get; set; }
        public string groupName { get; set; }
        public IGroup.SortDirection sortDirection { get; set; }
        Action<int, IGroup.SortDirection> onSort;

        void Awake()
        {
            sortDirectionButton.onClick.AddListener(ChnageSortDirection);
        }

        public void Deselect()
        {
            isSelect = false;
            bgImage.color = Definitions.DeselectColor;
        }

        public GameObject GetObject()
        {
            return gameObject;
        }

        public void Init(IPanelGroup.Param param)
        {
            groupName = param.name;
            hasChannels = param.hasChannels;
            groupIndex = param.groupIndex;
            onSort = param.onSort;

            groupNameText.text = groupName;

            SetInitRotation(param.sortDirection);
        }

        public void Select()
        {
            isSelect = true;
            bgImage.color = Definitions.SelectColor;
        }

        public void ChnageSortDirection()
        {
            int currentIndex = (int)sortDirection;
            int nextIndex = (currentIndex + 1) % (int)IGroup.SortDirection.Count;

            sortDirection = (IGroup.SortDirection)nextIndex;
            onSort?.Invoke(groupIndex, sortDirection);

            RectTransform rectTransform = sortDirectionButton.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.Rotate(0, 0, -90); // ZÃà È¸Àü
            }
        }

        public bool IsSelect()
        {
            return isSelect;
        }

        void SetInitRotation(IGroup.SortDirection sortDirection)
        {
            RectTransform rectTransform = sortDirectionButton.GetComponent<RectTransform>();
            if (rectTransform == null)
                return;

            switch (sortDirection)
            {
                case IGroup.SortDirection.Left:
                    rectTransform.localEulerAngles = Left;
                    break;
                case IGroup.SortDirection.Up:
                    rectTransform.localEulerAngles = Up;
                    break;
                case IGroup.SortDirection.Right:
                    rectTransform.localEulerAngles = Right;
                    break;
                case IGroup.SortDirection.Down:
                    rectTransform.localEulerAngles = Down;
                    break;
            }
        }
    }
}
