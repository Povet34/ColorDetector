using DataEdit;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EditDataStoreImp : IEditDataStore
{
    public Dictionary<int, Color32> colorSheetData { get; set; }
    public Dictionary<SavedChannelKey, SavedChannelValue> channelData { get; set; }

    public void Edit()
    {
    }

    public List<SavedChannelKey> GetChannelData()
    {
        return channelData.Keys.ToList();
    }

    public void Improt(SaveData data)
    {
        channelData = new Dictionary<SavedChannelKey, SavedChannelValue>(data.recordData);
        colorSheetData = new Dictionary<int, Color32>(data.colorSheetData);

        Bus<RefreshPanelArgs>.Raise(new RefreshPanelArgs());
    }
}
