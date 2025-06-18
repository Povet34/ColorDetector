using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public interface IEditDataStore
{
    string path { get; set; }
    OrderType orderType { get; set; }
    Dictionary<int, Color32> colorSheetData { get; set; }
    Dictionary<SavedChannelKey, SavedChannelValue> channelData { get; set; }
    Dictionary<int, List<ColorFlowReasoner.Step>> inferredColorData { get; set; }

    void Improt(ImportResult importResult);
    void UpdateRawDataAll(Dictionary<SavedChannelKey, SavedChannelValue> channelData);
    void UpdateRawData_OneChannel(int channelIndex, List<Color32> colors);

    List<SavedChannelKey> GetChannelData();
}