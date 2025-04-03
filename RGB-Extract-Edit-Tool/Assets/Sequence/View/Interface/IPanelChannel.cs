using UnityEngine;

namespace DataExtract
{
    public interface IPanelChannel
    {
        int channelIndex { get; set; }
        Vector2 position { get; set; }
        IPanelGroup parentGroup { get; set; }
        bool hasGroup { get; set; }
        int parentGroupIndex { get; set; }
        int groupInIndex { get; set; }

        //------

        GameObject GetObject();
        void MoveDelta(Vector2 deltaPos);
        void Move(Vector2 pos);
        void DestroyChannel();
        void Select();
        void Deselect();
        bool IsSelect();

        //------

        void SetGroup(IPanelGroup parentGroup, int groupInIndex);
    }
}