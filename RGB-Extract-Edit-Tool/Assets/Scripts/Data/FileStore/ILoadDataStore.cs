using System;
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
    ulong totalFrame { get; set; }

    RenderTexture videoTexture { get; set; }
    Action onUpdateVideoTexture { get; set; }

    void UpdateLoadedVideoData(string path);
}
