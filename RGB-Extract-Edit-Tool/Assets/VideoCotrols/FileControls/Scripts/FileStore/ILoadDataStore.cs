using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 추출한 데이터를 저장하는 Store
/// </summary>
public interface ILoadDataStore
{
    //Video Data
    string videoName { get; set; }
    string videoUrl { get; set; }
    float videoLength { get; set; }
    Vector2 videoResolution { get; set; }
    float videoFrameRate { get; set; }

    void UpdateLoadedVideoData(string path);
}
