using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

/// <summary> Sets a background color for game objects in the Hierarchy tab</summary>
[UnityEditor.InitializeOnLoad]
public class HierarchyObjectColor
{
    private static Vector2 offset = new Vector2(20, 1);

    private static List<string> childrenNames = new List<string>() { "HierarchyPanel", "VideoViewPanel" };
    private static List<string> rootNames = new List<string>();

    static HierarchyObjectColor()
    {
        EditorApplication.hierarchyWindowItemOnGUI += HandleHierarchyWindowItemOnGUI;
    }

    private static void HandleHierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
    {
        var obj = EditorUtility.InstanceIDToObject(instanceID);
        if (obj != null)
        {
            Color backgroundColor = Color.white;
            Color textColor = Color.white;
            Texture2D texture = null;

            if (rootNames.Contains(obj.name))
            {
                backgroundColor = new Color(0.6f, 0.2f, 0.1f);
                textColor = new Color(0.9f, 0.9f, 0.9f);
            }

            // Write your object name in the hierarchy.
            if (childrenNames.Contains(obj.name))
            {
                backgroundColor = new Color(0.2f, 0.6f, 0.1f);
                textColor = new Color(0.9f, 0.9f, 0.9f);
            }

            if (backgroundColor != Color.white)
            {
                Rect offsetRect = new Rect(selectionRect.position + offset, selectionRect.size);
                Rect bgRect = new Rect(selectionRect.x, selectionRect.y, selectionRect.width + 50, selectionRect.height);

                EditorGUI.DrawRect(bgRect, backgroundColor);
                EditorGUI.LabelField(offsetRect, obj.name, new GUIStyle()
                {
                    normal = new GUIStyleState() { textColor = textColor },
                    fontStyle = FontStyle.Bold
                });

                if (texture != null)
                    EditorGUI.DrawPreviewTexture(new Rect(selectionRect.position, new Vector2(selectionRect.height, selectionRect.height)), texture);
            }
        }
    }
}
#endif
