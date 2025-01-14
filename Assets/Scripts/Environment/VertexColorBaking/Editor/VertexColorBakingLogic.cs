using System;
using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class VertexColorBakingLogic
{
    public static void ClearVertexColorData(Transform[] targetObjects)
    {
        if (targetObjects == null) return;

        for (int i = 0; i < targetObjects.Length; i++)
        {
            var targetObject = targetObjects[i];
            EditorUtility.DisplayProgressBar("Vertex Color Baking", $"Clearing data ({i}/{targetObjects.Length})", i / (float)targetObjects.Length);
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
        EditorUtility.ClearProgressBar();
    }

    public static void BakeVertexColors(Transform[] targetObjects, VertexColorBakingSettings settings)
    {
        ClearVertexColorData(targetObjects);

        EditorUtility.DisplayProgressBar("Vertex Color Baking", "Gathering Lights", 0.0f);
        var lights = GetLights();

        for (int i = 0; i < targetObjects.Length; i++)
        {
            var targetObject = targetObjects[i];
            EditorUtility.DisplayProgressBar("Vertex Color Baking", $"Coloring objects ({i}/{targetObjects.Length})", i / (float)targetObjects.Length);
            ColorObject(targetObject);
        }

        EditorUtility.ClearProgressBar();

        VertexColorLightSource[] GetLights()
        {
            return targetObjects
                        .Select(o => o.GetComponent<VertexColorLightSource>())
                        .Where(o => o != null && o.isActiveAndEnabled)
                        .ToArray();
        }

        void ColorObject(Transform targetObject)
        {

            var meshFilter = targetObject.GetComponent<MeshFilter>();
            if (meshFilter == null) return;

            var bakedData = Undo.AddComponent<BakedVertexColorData>(targetObject.gameObject);
            Undo.RecordObject(bakedData, "Set vertex color data");

            var localVerts = meshFilter.sharedMesh.vertices.ToArray();
            var worldPositionVerts = localVerts.Select(v => targetObject.TransformPoint(v)).ToArray();

            // var colors = Enumerable.Repeat(settings.SkyColor, meshFilter.sharedMesh.vertices.Length).ToArray();
            var colors = worldPositionVerts.Select(v => GetColorForVert(v)).ToArray();

            bakedData.SetColors(colors);
        }

        Color GetColorForVert(Vector3 worldPos)
        {
            var color = settings.SkyColor;
            foreach (var light in lights)
            {
                var distance = Vector3.Distance(worldPos, light.transform.position);
                var power = settings.PointLightFalloff.Evaluate(distance / light.Range);
                power *= light.Intensity;
                if (power < .01f) continue;
                var targetColor = light.Color * color;
                color = Color.Lerp(color, targetColor, power);
            }
            return color;
        }
    }

}