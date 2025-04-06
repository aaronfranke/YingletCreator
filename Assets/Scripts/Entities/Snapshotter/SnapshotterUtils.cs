using System.Diagnostics;
using UnityEngine;

namespace Snapshotter
{
    public static class SnapshotterUtils
    {
        public static RenderTexture Snapshot(ISnapshotterReferences references, SnapshotterParams sParams)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            using var prefabHandler = new SnapshotterPrefabHandler(references, sParams);
            using var cameraHandler = new SnapshotterCameraHandler(references, sParams);


            var rt = new RenderTexture(references.SizeInPixels, references.SizeInPixels, 24);
            rt.Create();
            cameraHandler.RenderTo(rt);

            //UnityEngine.Debug.Log($"Snapshot took {stopWatch.ElapsedMilliseconds}ms");
            return rt;
        }
    }
}