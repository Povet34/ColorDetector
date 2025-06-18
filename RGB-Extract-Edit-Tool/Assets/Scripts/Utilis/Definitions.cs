using System.IO;
using UnityEngine;

public static class Definitions
{
    public static string SavePath(string name) => Path.Combine(
        System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), $"{name}.xlsx");

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

    public const int ZeroHeaderRow = 0;
    public const int FirstHeaderRow = 1; //ChannelData는 여기서 시작
    public const int SecondHeaderRow = 2;
    public const int LastHeader = 3;

    //Sheet NAME
    public const string Channel_Data = "Channel Data";
    public const string ColorPalette = "Color Palette";

    public const string OriginColor = "Origin Color";
    public const string O_OnOutRed = "O_OnOutRed";
    public const string O_OnOutGreen = "O_OnOutGreen";
    public const string O_OnOutBlue = "O_OnOutBlue";

    public const string ModifiedColor = "Modified Color";
    public const string M_OnOutRed = "M_OnOutRed";
    public const string M_OnOutGreen = "M_OnOutGreen";
    public const string M_OnOutBlue = "M_OnOutBlue";

    public const string InferredColor = "Inferred Color";   
}
