namespace ScenarioVerification
{
    public struct LoadFromExcelArgs
    {
    }

    public struct RefreshPanelArgs
    {
    }

    public struct PlayScenarioArgs
    {
        public int dataType; // 0이면 Origin, 1이면 Modified

        public PlayScenarioArgs(int dataType)
        {
            this.dataType = dataType;
        }
    }
}