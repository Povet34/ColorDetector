using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ColorFlowViewCell : MonoBehaviour
{
    public int channelIndex { get; private set; }

    [SerializeField] TMP_Text channelIndexText;
    [SerializeField] Image flowTextureImage;
    [SerializeField] Button startEditButton;

    public void Init(int channelIndex, string flowTexturePath, bool isOn)
    {
        this.channelIndex = channelIndex;
        channelIndexText.text = channelIndex.ToString();
    }
}
