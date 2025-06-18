using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class EditDataStoreImp : IEditDataStore
{
    public string path { get; set; }
    public OrderType orderType { get; set; }
    public Dictionary<int, Color32> colorSheetData { get; set; }
    public Dictionary<SavedChannelKey, SavedChannelValue> channelData { get; set; }
    public Dictionary<int, List<ColorFlowReasoner.Step>> inferredColorData { get; set; }

    public List<SavedChannelKey> GetChannelData()
    {
        return channelData.Keys.ToList();
    }

    public void Improt(ImportResult importResult)
    {
        path = importResult.path;

        if (importResult.additionalData.modifiedRecordData != null && importResult.additionalData.modifiedRecordData.Count > 0)
            channelData = importResult.additionalData.modifiedRecordData;
        else
            channelData = importResult.originData.recordData;

        colorSheetData = importResult.originData.colorSheetData;
        inferredColorData = importResult.inferredColorFlowData.inferredColorFlow;
    }

    public void UpdateRawDataAll(Dictionary<SavedChannelKey, SavedChannelValue> channelData)
    {
        this.channelData = channelData;
    }

    public void UpdateRawData_OneChannel(int channelIndex, List<Color32> colors)
    {
        var key = channelData.Keys.FirstOrDefault(k => k.index == channelIndex);
        if (!key.Equals(default(SavedChannelKey)))
        {
            channelData[key] = new SavedChannelValue { colors = colors };
        }
    }
}
