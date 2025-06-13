using System.Collections.Generic;
using UnityEngine;

public interface IImportFrom
{
    Dictionary<SavedChannelKey, SavedChannelValue> Import(string path);
    List<SavedChannelKey> LoadChannelInfos(string path);
}
