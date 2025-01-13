using System.Collections;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class VertexColorBakingLogic
{
    public static void ClearVertexColorData(Transform[] targetObjects)
    {
        if (targetObjects == null) return;

        foreach (var targetObject in targetObjects)
        {
            // var vdColorHandler = targetObject.GetComponent<VDColorHandler>();
            // if (vdColorHandler != null)
            // {
            //     Undo.DestroyObjectImmediate(vdColorHandler);
            // }
            var bakedData = targetObject.GetComponent<BakedVertexColorData>();
            if (bakedData != null)
            {
                Undo.DestroyObjectImmediate(bakedData);
            }
        }
    }

    public static void BakeVertexColors(Transform[] targetObjects, VertexColorBakingSettings settings)
    {
        ClearVertexColorData(targetObjects);
        foreach (var targetObject in targetObjects)
        {
            var meshFilter = targetObject.GetComponent<MeshFilter>(); 
            if (meshFilter == null) continue;


            var bakedData = Undo.AddComponent<BakedVertexColorData>(targetObject.gameObject);
            Undo.RecordObject(bakedData, "Set vertex color data");
            var colors = Enumerable.Repeat(settings.SkyColor, meshFilter.sharedMesh.vertices.Length).ToArray();
            bakedData.SetColors(colors);
        }
    }
}
