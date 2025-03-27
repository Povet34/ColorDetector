using UnityEngine;

public static class TransformEx
{
    /// <summary>
    /// screen 좌표를 특정 RT의 자식 좌표로 반환해준다. 
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
