using UnityEngine;

namespace Snapshotter
{
    public static class SnapshotterUtils
    {
        public static RenderTexture Snapshot(ISnapshotterReferences references, SnapshotterParams sParams)
        {
            using var prefabHandler = new SnapshotterPrefabHandler(references);
            using var cameraHandler = new SnapshotterCameraHandler(references, sParams);


            var rt = new RenderTexture(references.SizeInPixels, references.SizeInPixels, 24);
            rt.Create();
            cameraHandler.RenderTo(rt);
            return rt;
        }
    }
}