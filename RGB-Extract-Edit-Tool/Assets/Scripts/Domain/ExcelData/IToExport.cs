using System.Collections.Generic;
using UnityEngine;

public interface IToExport
{
    void Export(Dictionary<SavedChannelKey, SavedChannelValue> data, string filePath);
}
