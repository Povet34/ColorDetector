namespace DataEdit
{
    public struct ChannelShowToggleEventArgs
    {
        public int channelIndex { get; }
        public bool IsOn { get; }
        public ChannelShowToggleEventArgs(int channelIndex, bool isOn)
        {
            this.channelIndex = channelIndex;
            IsOn = isOn;
        }
    }

    public struct LoadFromExcelEventArgs
    {
        public string filePath { get; }
        public LoadFromExcelEventArgs(string filePath)
        {
            this.filePath = filePath;
        }
    }

    public struct RefreshPanelArgs
    {
    }
}

