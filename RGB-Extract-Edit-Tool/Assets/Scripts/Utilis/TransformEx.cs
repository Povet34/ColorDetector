using UnityEngine;

public static class TransformEx
{
    /// <summary>
    /// screen 좌표를 특정 RT의 자식 좌표로 반환해준다. Bottom Left 기준
    /// </summary>
    /// <param name="pannel"></param>
    /// <param name="screenPosition"></param>
    /// <returns></returns>
    public static Vector2 GetRelativeAnchorPosition_Screen(RectTransform pannel, Vector2 screenPosition)
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(pannel, screenPosition, null, out localPoint);

        // 앵커를 Bottom-Left 기준으로 변환
        Vector2 bottomLeftOffset = new Vector2(pannel.rect.width * pannel.pivot.x, pannel.rect.height * pannel.pivot.y);
        Vector2 bottomLeftPosition = localPoint + bottomLeftOffset;

        return bottomLeftPosition;
    }
}
