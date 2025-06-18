using UnityEngine;
using System.Collections.Generic;

namespace DataEdit
{
    public struct ChannelShowToggleArgs
    {
        public int channelIndex { get; }
        public bool IsOn { get; }
        public ChannelShowToggleArgs(int channelIndex, bool isOn)
        {
            this.channelIndex = channelIndex;
            IsOn = isOn;
        }
    }

    public struct LoadFromExcelArgs
    {
        public int loadType; // 0 : inferred, 1 : raw

        public LoadFromExcelArgs(int loadType)
        {
            this.loadType = loadType;
        }
    }

    public struct RefreshPanelArgs
    {
        public int loadType; // 0 : inferred, 1 : raw

        public RefreshPanelArgs(int loadType)
        {
            this.loadType = loadType;
        }
    }

    public struct StartEditArgs
    {
        public int channelIndex;
        public List<Color32> colors;
        public List<Color32> palette; //이건 없어도될듯?

        public StartEditArgs(int channelIndex, List<Color32> colors, List<Color32> palette)
        {
            this.channelIndex = channelIndex;
            this.colors = colors;
            this.palette = palette;
        }
    }

    public struct SaveEditArgs
    {
        public int channelIndex;
        public List<Color32> colors;

        public SaveEditArgs(int channelIndex, List<Color32> colors)
        {
            this.channelIndex = channelIndex;
            this.colors = colors;
        }
    }
}

