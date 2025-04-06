using UnityEditor;

namespace Snapshotter
{
    public sealed class SnapshotterTest
    {
        const string ReferencesRelativePath = "Assets/Scripts/Entities/Snapshotter/SnapshotterReferences.asset";
        const string CameraPosRelativePath = "Assets/Scripts/Entities/Snapshotter/CameraPositions/_Default.asset";

        [MenuItem("Custom/Test Snapshotter")]
        public static void TestSnapshotter()
        {
            var references = AssetDatabase.LoadAssetAtPath<SnapshotterReferences>(ReferencesRelativePath);
            var camPos = AssetDatabase.LoadAssetAtPath<SnapshotterCameraPosition>(CameraPosRelativePath);
            var sParams = new SnapshotterParams(camPos);
            SnapshotterUtils.Snapshot(references, sParams);
        }
    }
}