using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public interface IEditDataStore
{
    Dictionary<int, Color32> colorSheetData { get; set; }
    Dictionary<SavedChannelKey, SavedChannelValue> channelData { get; set; }

    void Improt(SaveData data);
    void Edit();

    List<SavedChannelKey> GetChannelData();
}