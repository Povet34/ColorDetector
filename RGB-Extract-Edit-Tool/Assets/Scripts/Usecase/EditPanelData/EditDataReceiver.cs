using System.Collections.Generic;
using UnityEngine;

public class EditDataReceiver
{
    IEditDataStore editDataStore;
    public EditDataReceiver(IEditDataStore editDataStore)
    {
        this.editDataStore = editDataStore;
    }

    public List<SavedChannelKey> GetChannelData()
    {
        return editDataStore.GetChannelData();
    }

    public Dictionary<SavedChannelKey, SavedChannelValue> GetRecordedData()
    {
        return editDataStore.channelData;
    }

    public Dictionary<int, Color32> GetColorSheet()
    {
        return editDataStore.colorSheetData;
    }
}
