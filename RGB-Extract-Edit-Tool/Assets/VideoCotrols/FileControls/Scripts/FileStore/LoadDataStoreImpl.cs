using System.IO;
using UnityEngine;

public class LoadDataStoreImpl : ILoadDataStore
{
    public string videoName { get; set; }
    public string videoUrl { get; set; }
    public float videoLength { get; set; }
    public Vector2 videoResolution { get; set; }
    public float videoFrameRate { get; set; }

    public void StoreExtractData()
    {
        throw new System.NotImplementedException();
    }

    public void UpdateLoadedVideoData(string path)
    {
        videoUrl = path;
        videoName = Path.GetFileName(path);

        // VideoPlayer를 사용하여 메타데이터 추출
        var videoPlayer = new GameObject("VideoPlayer").AddComponent<UnityEngine.Video.VideoPlayer>();
        videoPlayer.url = path;

        videoPlayer.Prepare();
        videoPlayer.prepareCompleted += (source) =>
        {
            videoLength = (float)videoPlayer.length;
            videoResolution = new Vector2(videoPlayer.width, videoPlayer.height);
            videoFrameRate = videoPlayer.frameRate;

            Debug.Log($"Video Loaded: {videoName}, Length: {videoLength}s, Resolution: {videoResolution}, FrameRate: {videoFrameRate}fps");

            Object.Destroy(videoPlayer.gameObject); // VideoPlayer 객체 제거
        };
    }
}
