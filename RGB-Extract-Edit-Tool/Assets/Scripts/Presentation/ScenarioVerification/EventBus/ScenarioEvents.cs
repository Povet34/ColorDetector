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
        public int dataType; // 0�̸� Origin, 1�̸� Modified

        public PlayScenarioArgs(int dataType)
        {
            this.dataType = dataType;
        }
    }
}