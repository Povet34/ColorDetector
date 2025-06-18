using System.Collections.Generic;
using UnityEngine;

public interface IToExport
{
    void ExportNew(OriginData data, string filePath);
    void ExportAdd(AdditionalData data, string filePath);
}
