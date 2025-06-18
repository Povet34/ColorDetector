using System.Collections.Generic;
using UnityEngine;

public class EditDataReceiver
{
    IEditDataStore editDataStore;
    public EditDataReceiver(IEditDataStore editDataStore)
    {
        this.editDataStore = editDataStore;
    }
    public OrderType GetOrderType()
    {
        return editDataStore.orderType;
    }

    public List<SavedChannelKey> GetChannelData()
    {
        return editDataStore.GetChannelData();
    }

    public Dictionary<SavedChannelKey, SavedChannelValue> GetRecordedData()
    {
        return editDataStore.channelData;
    }

    public Dictionary<int, List<ColorFlowReasoner.Step>> GetInferredColorData()
    {
        return editDataStore.inferredColorData;
    }

    public Dictionary<int, Color32> GetColorSheet()
    {
        return editDataStore.colorSheetData;
    }

    public string GetCurrentEditDataPath()
    {
        return editDataStore.path;
    }
}
