using UnityEngine;

public class Brush
{
    Color currentColor;

    public Color GetColor()
    {
        return currentColor;
    }

    public void SetBrushColor(Color color)
    {
        currentColor = color;
    }
}
