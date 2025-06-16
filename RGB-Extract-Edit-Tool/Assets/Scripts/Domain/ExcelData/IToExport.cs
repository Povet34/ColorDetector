using System.Collections.Generic;
using UnityEngine;

public interface IToExport
{
    void Export(SaveData data, string filePath);
}
