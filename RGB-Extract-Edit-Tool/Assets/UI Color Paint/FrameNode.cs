using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public partial class FrameNode : MonoBehaviour, IPaintable
{
    [SerializeField] TextMeshProUGUI indexText;
    Image paintImage;

    public Color32 Color { get; set; }
    public int index { get; set; }
    public bool isChanged { get; set; } = false;

    public void Paint(Color32 color)
    {
        if(!paintImage)
        {
            paintImage = GetComponent<Image>();
        }

        paintImage.color = color;
        Color = color;
        isChanged = true;
    }

    public void Init(InitParam initParam)
    {
        Color = initParam.color;
        index = initParam.index;

        Paint(Color);
        SetIndexText(index);

        name = $"FrameNode_{index}";
    }

    private void SetIndexText(int index)
    {
        indexText.text = index.ToString();
    }
}
