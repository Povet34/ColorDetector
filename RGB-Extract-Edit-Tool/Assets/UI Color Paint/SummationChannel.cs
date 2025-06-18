using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SummationChannel : SummationView
{
    [Serializable]
    public class ChannelData
    {
        public int channelIndex;
        public List<Color32> colors;
        public List<PaletteColor> paletteColors;
        public ChannelEditPanel channelEditPanel;
    }

    [SerializeField] TextMeshProUGUI channelIndexText;
    [SerializeField] Button editButton;
    [SerializeField] RawImage colorViewImage;

    private ChannelEditPanel channelEditPanel;
    private int channelIndex;

    private void Awake()
    {
        editButton.onClick.AddListener(StartEdit);
    }

    public void SetData(ChannelData channelData) 
    {
        colors = channelData.colors;
        channelIndex = channelData.channelIndex;
        channelEditPanel = channelData.channelEditPanel;

        channelIndexText.text = channelIndex.ToString();
        colorViewImage.texture = CreateColorArrayTexture(colors);
    }

    private void StartEdit()
    {
        //ChannelEditPanel.Data data = new ChannelEditPanel.Data();
        //data.colors = colors;
        //data.channelIndex = channelIndex;
        //data.repaintCallback = RepaintTexture;
        
        //channelEditPanel.StartEdit(data);
    }

    private void RepaintTexture(List<Color32> colors)
    {
        colorViewImage.texture = CreateColorArrayTexture(colors);
    }
}
