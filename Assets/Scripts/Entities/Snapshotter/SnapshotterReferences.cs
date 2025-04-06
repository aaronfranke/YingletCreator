using UnityEngine;

namespace Snapshotter
{
    public interface ISnapshotterReferences
    {
        GameObject YingletPrefab { get; }
        int LayerIndex { get; }
        int LayerMask { get; }

        /// <summary>
        /// Mostly used for testing
        /// </summary>
        bool CleanupObjects { get; }
    }

    [CreateAssetMenu(fileName = "SnapshotterReferences", menuName = "Scriptable Objects/Misc/SnapshotterReferences")]
    public sealed class SnapshotterReferences : ScriptableObject, ISnapshotterReferences
    {
        [SerializeField] GameObject _yingletPrefab;
        [SerializeField] int _layerIndex;
        [SerializeField] bool _cleanupObject;

        public GameObject YingletPrefab => _yingletPrefab;
        public int LayerIndex => _layerIndex;
        public int LayerMask => 1 << _layerIndex;

        public bool CleanupObjects => _cleanupObject;
    }
}
