using System;
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
    ulong totalFrame { get; set; }

    RenderTexture videoTexture { get; set; }
    Action onUpdateVideoTexture { get; set; }

    void UpdateLoadedVideoData(string path);
}
