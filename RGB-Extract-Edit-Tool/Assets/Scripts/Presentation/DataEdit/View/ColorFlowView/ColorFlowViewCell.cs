using DataEdit;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static ColorFlowViewCell;

public class ColorFlowViewCell : MonoBehaviour
{
    public struct CreateInfo
    {
        public int channelIndex;
        public List<int> colorIndices;
        public Dictionary<int, Color32> colorPalette;
        public List<Color32> rawColors;
    }

    public int channelIndex { get; private set; }
    public List<int> colorIndices { get; private set; }
    public Dictionary<int, Color32> colorPalette { get; private set; }
    public List<Color32> rawColors { get; private set; }


    [SerializeField] TMP_Text channelIndexText;
    [SerializeField] Image flowTextureImage;
    [SerializeField] Button startEditButton;

    private void Awake()
    {
        startEditButton.onClick.AddListener(StartEdit);
    }

    private void OnDestroy()
    {
        startEditButton.onClick.RemoveListener(StartEdit);
    }

    public void Init(CreateInfo createInfo)
    {
        channelIndex = createInfo.channelIndex;
        colorIndices = createInfo.colorIndices;
        colorPalette = createInfo.colorPalette;

        channelIndexText.text = channelIndex.ToString();
        
        SetTexture();
    }

    public void InitRaw(CreateInfo createInfo)
    {
        channelIndex = createInfo.channelIndex;
        colorIndices = createInfo.colorIndices;
        colorPalette = createInfo.colorPalette;
        rawColors = createInfo.rawColors;

        channelIndexText.text = channelIndex.ToString();

        SetTextureRaw(rawColors);
    }

    private void SetTexture()
    {
        if (colorIndices == null || colorIndices.Count == 0 || colorPalette == null)
            return;

        int width = colorIndices.Count;
        int height = 1;
        Texture2D tex = new Texture2D(width, height, TextureFormat.RGBA32, false);
        tex.wrapMode = TextureWrapMode.Clamp;
        tex.filterMode = FilterMode.Point;

        for (int x = 0; x < width; x++)
        {
            int colorIndex = colorIndices[x];
            Color32 color;
            if (colorPalette.TryGetValue(colorIndex, out color))
            {
                tex.SetPixel(x, 0, color);
            }
            else
            {
                tex.SetPixel(x, 0, new Color32(0, 0, 0, 0)); // 없는 경우 투명
            }
        }

        tex.Apply();
        SetSprite(tex, width, height);
    }

    private void SetTextureRaw(List<Color32> colors)
    {
        if (colors == null || colors.Count == 0)
            return;

        int width = colors.Count;
        int height = 1;
        Texture2D tex = new Texture2D(width, height, TextureFormat.RGBA32, false);
        tex.wrapMode = TextureWrapMode.Clamp;
        tex.filterMode = FilterMode.Point;

        for (int x = 0; x < width; x++)
        {
            tex.SetPixel(x, 0, colors[x]);
        }

        tex.Apply();
        SetSprite(tex, width, height);
    }

    void SetSprite(Texture2D tex, int width, int height)
    {
        Rect rect = new Rect(0, 0, width, height);
        Vector2 pivot = new Vector2(0.0f, 0.5f);
        Sprite sprite = Sprite.Create(tex, rect, pivot, 1f);

        flowTextureImage.sprite = sprite;
    }

    void StartEdit()
    {
        Bus<StartEditArgs>.Raise(
            new StartEditArgs(channelIndex, rawColors, colorPalette.Values.ToList())
            );
    }
}
