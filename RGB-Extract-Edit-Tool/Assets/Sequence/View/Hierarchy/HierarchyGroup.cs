using TMPro;
using UnityEngine;
using System.Collections.Generic;
using System.Xml.Linq;

namespace DataExtract
{

    public class HierarchyGroup : MonoBehaviour
    {
        [SerializeField] TMP_Text groupNameText;
        string groupName;
        List<int> channelIndices;

        public void Init(MakeGroupParam param)
        {
            groupName = param.name;
            channelIndices = param.channelIndices;

            groupNameText.text = groupName;
        }
    }
}
