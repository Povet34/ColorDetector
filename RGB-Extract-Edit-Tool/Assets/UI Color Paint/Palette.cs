using System.Collections.Generic;
using UnityEngine;

public class Palette : MonoBehaviour
{
    [SerializeField] PaletteColor paletteColorPrefab;

    List<PaletteColor> paletteColors = new List<PaletteColor>();

    public void Init(List<Color32> colors, Brush brush)
    {
        // ���� ������Ʈ ����
        foreach (var paletteColor in paletteColors)
        {
            if (paletteColor != null)
                Destroy(paletteColor.gameObject);
        }
        paletteColors.Clear();

        // �� �÷� ����
        foreach (var color in colors)
        {
            var obj = Instantiate(paletteColorPrefab, transform);
            obj.Init(color, brush);
            paletteColors.Add(obj);
        }
    }

    public List<PaletteColor> GetPaletteColors()
    {
        return paletteColors;
    }   
}
