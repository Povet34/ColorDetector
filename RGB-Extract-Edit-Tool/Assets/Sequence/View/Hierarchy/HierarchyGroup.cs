using TMPro;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

namespace DataExtract
{

    public class HierarchyGroup : MonoBehaviour, IPanelGroup
    {
        [SerializeField] TMP_Text groupNameText;
        [SerializeField] Image bgImage;

        public List<IPanelChannel> hasChannels { get; set; }
        public int groupIndex { get; set; }
        public string groupName { get; set; }

        public void Deselect()
        {
            bgImage.color = new Color32(100, 100, 100, 255);
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

            groupNameText.text = groupName;
        }

        public void Select()
        {
            bgImage.color = new Color32(200, 50, 50, 255);
        }
    }
}
