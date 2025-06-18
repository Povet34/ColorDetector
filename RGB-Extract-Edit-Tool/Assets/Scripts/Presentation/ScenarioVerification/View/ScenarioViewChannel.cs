using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

public class ScenarioViewChannel : MonoBehaviour
{
    public int channelIndex { get; set; }
    [SerializeField] Image img; 

    public void Init(Vector2 pos) 
    {
        transform.position = pos;
        ResetColor();
    }

    public void SetColor(Color32 color)
    {
        img.color = color;
    }

    public void ResetColor()
    {
        SetColor(Color.white);
    }
}
