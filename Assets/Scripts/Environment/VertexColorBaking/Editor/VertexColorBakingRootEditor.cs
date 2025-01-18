using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(VertexColorBakingRoot))]
public class VertexColorBakingRootEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var root = target as VertexColorBakingRoot;

        if (GUILayout.Button("Clear Vertex Color Data"))
        {
            VertexColorBakingLogic.ClearVertexColorData(root.transform);
        }
        GUILayout.Space(5);

        if (GUILayout.Button("Bake"))
        {
            var selection = Selection.GetTransforms(SelectionMode.Deep);
            VertexColorBakingLogic.BakeVertexColors(root.transform, root._settings);
        }
    }
}