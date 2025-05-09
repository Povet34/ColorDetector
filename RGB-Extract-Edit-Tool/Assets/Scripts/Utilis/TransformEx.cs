using UnityEngine;

public static class TransformEx
{
    /// <summary>
    /// screen ��ǥ�� Ư�� RT�� �ڽ� ��ǥ�� ��ȯ���ش�. Bottom Left ����
    /// </summary>
    /// <param name="pannel"></param>
    /// <param name="screenPosition"></param>
    /// <returns></returns>
    public static Vector2 GetRelativeAnchorPosition_Screen(RectTransform pannel, Vector2 screenPosition)
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(pannel, screenPosition, null, out localPoint);

        // ��Ŀ�� Bottom-Left �������� ��ȯ
        Vector2 bottomLeftOffset = new Vector2(pannel.rect.width * pannel.pivot.x, pannel.rect.height * pannel.pivot.y);
        Vector2 bottomLeftPosition = localPoint + bottomLeftOffset;

        return bottomLeftPosition;
    }
}
