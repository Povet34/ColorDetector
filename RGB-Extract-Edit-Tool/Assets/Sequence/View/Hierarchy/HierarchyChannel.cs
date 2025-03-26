using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HierarchyChannel : MonoBehaviour, 
    IPanelChannel
{
    public int channelIndex { get; set; }
    public Vector2 position { get; set; }

    [SerializeField] TMP_Text channelText;
    [SerializeField] TMP_InputField inputX;
    [SerializeField] TMP_InputField inputY;

    public void Init()
    {
    }

    public void Move()
    {
    }

    public void Destroy()
    {
    }
}
