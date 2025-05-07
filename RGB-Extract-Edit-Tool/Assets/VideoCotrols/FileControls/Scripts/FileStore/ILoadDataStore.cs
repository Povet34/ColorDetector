using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������ �����͸� �����ϴ� Store
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
