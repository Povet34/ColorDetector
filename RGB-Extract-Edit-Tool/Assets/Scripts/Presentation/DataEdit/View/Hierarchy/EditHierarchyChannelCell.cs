using DataExtract;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DataEdit
{
    public class EditHierarchyChannelCell : MonoBehaviour
    {
        int channelIndex;

        [SerializeField] TMP_Text channelIndexText;
        [SerializeField] TMP_Text groupIndexText;
        [SerializeField] TMP_Text groupInindexText;
        [SerializeField] Toggle showToggle;

        public void Init(int channelIndex, int groupIndex, int groupInindex, bool isOn)
        {
            this.channelIndex = channelIndex;

            channelIndexText.text = channelIndex.ToString();
            groupIndexText.text = groupIndex.ToString();
            groupInindexText.text = groupInindex.ToString();
            showToggle.isOn = isOn;
        }

        void Awake()
        {
            if (showToggle != null)
                showToggle.onValueChanged.AddListener(OnShowToggleChanged);
        }

        void OnDestroy()
        {
            if (showToggle != null)
                showToggle.onValueChanged.RemoveListener(OnShowToggleChanged);
        }

        private void OnShowToggleChanged(bool isOn)
        {
            Bus<ChannelShowToggleEventArgs>.Raise(new ChannelShowToggleEventArgs(channelIndex, isOn));
        }
    }
}