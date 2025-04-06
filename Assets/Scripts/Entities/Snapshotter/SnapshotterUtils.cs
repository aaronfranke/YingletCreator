namespace Snapshotter
{
    public static class SnapshotterUtils
    {
        public static void Snapshot(ISnapshotterReferences references, SnapshotterParams sParams)
        {
            using var prefabHandler = new SnapshotterPrefabHandler(references);
            using var cameraHandler = new SnapshotterCameraHandler(references, sParams);
        }
    }
}