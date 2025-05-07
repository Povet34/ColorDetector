using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������ �����͸� �����ϴ� Store
/// </summary>
public interface ILoadDataStore
{
    //Video Data
    string fileName { get; set; }
    string filePath { get; set; }
    float fileLength { get; set; }
    Vector2 fileResolution { get; set; }
    float fileFrameRate { get; set; }

    void Init();
    void SetVideo(string path);
}
