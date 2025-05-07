using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 추출한 데이터를 저장하는 Store
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
