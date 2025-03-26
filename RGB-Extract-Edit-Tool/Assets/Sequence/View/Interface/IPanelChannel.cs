using UnityEngine;
public interface IPanelChannel
{
    int channelIndex { get; set; }
    Vector2 position { get; set; }

    void Init();
    void Move();
    void Destroy();
}
