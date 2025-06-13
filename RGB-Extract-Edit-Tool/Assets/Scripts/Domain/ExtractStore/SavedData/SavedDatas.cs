using System.Collections.Generic;
using UnityEngine;

public struct SavedChannelKey
{
    public int index;
    public Vector2 position;
    public string groupName;
    public int groupInindex;
    public int sortDirection;
}

public struct SavedChannelValue
{
    public List<Color32> colors;
}

public struct SavedColorData
{
    public int index;
    public Color32 color;
}