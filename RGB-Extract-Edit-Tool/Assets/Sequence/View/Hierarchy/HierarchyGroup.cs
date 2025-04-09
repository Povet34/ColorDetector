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
            throw new System.NotImplementedException();
        }

        public GameObject GetObject()
        {
            return gameObject;
        }

        public void Init(IPanelGroup.Param param)
        {
            groupName = param.name;
            hasChannels = param.hasChannels;

            groupNameText.text = groupName;
        }

        public void Select()
        {
            throw new System.NotImplementedException();
        }
    }
}
