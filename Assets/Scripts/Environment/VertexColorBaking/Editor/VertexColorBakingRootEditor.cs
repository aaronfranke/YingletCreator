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

        if (GUILayout.Button("Clear Vertex Color Data (all)"))
        {
            var allBakingRoots = FindObjectsByType<VertexColorBakingRoot>(FindObjectsSortMode.None);
            foreach (var bakingRoot in allBakingRoots)
            {
                VertexColorBakingLogic.ClearVertexColorData(bakingRoot.transform);
            }
        }
        GUILayout.Space(5);

        if (GUILayout.Button("Bake"))
        {
            VertexColorBakingLogic.BakeVertexColors(root.transform, root._settings);
        }
        GUILayout.Space(5);

        if (GUILayout.Button("Bake (all)"))
        {

            var allBakingRoots = FindObjectsByType<VertexColorBakingRoot>(FindObjectsSortMode.None);
            foreach (var bakingRoot in allBakingRoots)
            {
                VertexColorBakingLogic.BakeVertexColors(bakingRoot.transform, bakingRoot._settings);
            }
        }
        GUILayout.Space(5);

    }
}