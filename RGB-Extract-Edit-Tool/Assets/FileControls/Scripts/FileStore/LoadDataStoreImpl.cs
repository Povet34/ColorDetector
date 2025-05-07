using UnityEngine;

public class LoadDataStoreImpl : ILoadDataStore
{
    public string fileName { get; set; }
    public string filePath { get; set; }
    public float fileLength { get; set; }
    public Vector2 fileResolution { get; set; }
    public float fileFrameRate { get; set; }

    public void StoreExtractData()
    {
        throw new System.NotImplementedException();
    }

    public void Init()
    {
    }

    public void SetVideo(string path)
    {
        throw new System.NotImplementedException();
    }
}
