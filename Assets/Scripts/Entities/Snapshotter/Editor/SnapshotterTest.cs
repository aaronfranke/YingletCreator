using System.IO;
using UnityEditor;
using UnityEngine;

namespace Snapshotter
{
    public sealed class SnapshotterTest
    {
        const string ReferencesRelativePath = "Assets/Scripts/Entities/Snapshotter/SnapshotterReferences.asset";
        const string CameraPosRelativePath = "Assets/Scripts/Entities/Snapshotter/CameraPositions/_Default.asset";
        const string OutputPath = "Assets/Scripts/Entities/Snapshotter/_TestGenerated.png";

        [MenuItem("Custom/Test Snapshotter")]
        public static void TestSnapshotter()
        {
            var references = AssetDatabase.LoadAssetAtPath<SnapshotterReferences>(ReferencesRelativePath);
            var camPos = AssetDatabase.LoadAssetAtPath<SnapshotterCameraPosition>(CameraPosRelativePath);
            Debug.LogError("TODO: Setup sparams again if I want to call this; should read from streaming assets and convert?");
            var sParams = new SnapshotterParams(camPos, null);
            var rt = SnapshotterUtils.Snapshot(references, sParams);

            // Create Texture2D and read pixels
            RenderTexture.active = rt;
            Texture2D tex = new Texture2D(rt.width, rt.height, TextureFormat.RGBA32, false);
            tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
            tex.Apply();
            RenderTexture.active = null;

            // Encode to PNG
            byte[] pngData = tex.EncodeToPNG();
            File.WriteAllBytes(OutputPath, pngData);
            Debug.Log("Saved PNG to: " + OutputPath);

            // Refresh AssetDatabase to show the new file
            AssetDatabase.Refresh();

            // Cleanup
            GameObject.DestroyImmediate(rt);
            GameObject.DestroyImmediate(tex);
        }
    }
}