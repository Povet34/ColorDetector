using UnityEngine;

public interface IPaintable
{
    Color32 Color { get; set; }
    void Paint(Color32 color);
}
