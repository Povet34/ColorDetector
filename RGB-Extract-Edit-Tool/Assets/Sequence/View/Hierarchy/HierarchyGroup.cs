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

        string groupName;
        public List<int> channelIndices { get; set; }
        public int groupIndex { get; set; }

        public void Deselect()
        {
            throw new System.NotImplementedException();
        }

        public GameObject GetObject()
        {
            return gameObject;
        }

        public void Init(MakeGroupParam param)
        {
            groupName = param.name;
            channelIndices = param.channelIndices;

            groupNameText.text = groupName;
        }

        public void Select()
        {
            throw new System.NotImplementedException();
        }
    }
}
