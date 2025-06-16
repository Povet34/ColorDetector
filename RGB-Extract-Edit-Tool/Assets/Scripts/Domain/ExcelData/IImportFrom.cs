using System.Collections.Generic;
using UnityEngine;

public interface IImportFrom
{
    SaveData Import(string path);
    List<SavedChannelKey> LoadChannelInfos(string path);
}
