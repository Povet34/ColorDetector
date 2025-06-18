using System.Collections.Generic;
using UnityEngine;

public interface IImportFrom
{
    ImportResult Import(string path);
    List<SavedChannelKey> LoadChannelInfos(string path);
}
