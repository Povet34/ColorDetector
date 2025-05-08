using UnityEngine;

public class ExtractTextureChanger
{
    private Texture2D cachedTexture2D; // 캐싱된 Texture2D

    public Texture2D ChangeTexture(RenderTexture renderTexture)
    {
        if (renderTexture == null)
        {
            Debug.LogError("RenderTexture is null. Cannot extract.");
            return null;
        }

        // RenderTexture 크기에 맞는 Texture2D 생성 또는 재사용
        if (cachedTexture2D == null || cachedTexture2D.width != renderTexture.width || cachedTexture2D.height != renderTexture.height)
        {
            if (cachedTexture2D != null)
            {
                Object.Destroy(cachedTexture2D); // 기존 Texture2D 해제
            }

            cachedTexture2D = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGBA32, false);
            Debug.Log($"New Texture2D created with resolution: {renderTexture.width}x{renderTexture.height}");
        }

        // RenderTexture의 내용을 Texture2D로 복사
        RenderTexture currentRT = RenderTexture.active;
        RenderTexture.active = renderTexture;

        cachedTexture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        cachedTexture2D.Apply();

        RenderTexture.active = currentRT;

        return cachedTexture2D;
    }

    public void ReleaseCache()
    {
        cachedTexture2D = null;
    }
}