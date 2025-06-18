using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PaletteColor : MonoBehaviour, IPointerDownHandler
{
    Image paletteColor;
    Brush brush;

    public void Init(Color color, Brush brush)
    {
        this.brush = brush;
        paletteColor = GetComponent<Image>();
        paletteColor.color = color;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        brush.SetBrushColor(paletteColor.color);
    }
}
