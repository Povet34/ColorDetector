using UnityEngine;

public static class TransformEx
{
    /// <summary>
    /// screen ��ǥ�� Ư�� RT�� �ڽ� ��ǥ�� ��ȯ���ش�. 
    /// </summary>
    /// <param name="pannel"></param>
    /// <param name="screenPosition"></param>
    /// <returns></returns>
    public static Vector2 GetRelativeAnchorPosition_Screen(RectTransform pannel, Vector2 screenPosition)
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(pannel, screenPosition, null, out localPoint);

        return localPoint;
    }
}
