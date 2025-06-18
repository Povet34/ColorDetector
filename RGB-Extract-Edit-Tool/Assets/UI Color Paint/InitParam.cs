using UnityEngine;

public partial class FrameNode
{
    public struct InitParam
    {
        public int index;
        public Color32 color;

        public InitParam(int index, Color32 color)
        {
            this.index = index;
            this.color = color;
        }
    }
}
