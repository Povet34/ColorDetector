using UnityEngine;

public static class Definitions
{
    //Color
    public static readonly Color32 SelectColor = new Color32(200, 50, 50, 255);
    public static readonly Color32 DeselectColor = new Color32(200, 200, 200, 255);
    public static readonly Color32 ChangeSelectColor = new Color32(255, 50, 50, 100);

    //Group
    public static string GetDefaultGroupName(string prefix)
    {
        return $"{prefix} NewGroup";
    }

    public static readonly Vector2 VideoViewPanelSize = new Vector2(1520, 940);
    public static readonly Vector2 VideoViewPanelHalfSize = VideoViewPanelSize / 2f;

    public static int GetPerceivedBrightness(Color32 color) => Mathf.RoundToInt(0.299f * color.r + 0.587f * color.g + 0.114f * color.b);
    public static float GetPerceivedBrightness(Color color) => (0.299f * color.r + 0.587f * color.g + 0.114f * color.b);


    //Sheet

    public const int GroupHeaderRow = 0;
    public const int FirstHeaderRow = 1;
    public const int SecondHeaderRow = 2;
    public const int LastHeader = 3;
}
