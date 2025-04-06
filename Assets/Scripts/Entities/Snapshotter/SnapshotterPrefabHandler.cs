using System;
using UnityEngine;

namespace Snapshotter
{
    internal sealed class SnapshotterPrefabHandler : IDisposable
    {
        private readonly ISnapshotterReferences _references;
        private readonly GameObject _yingletInstance;

        public SnapshotterPrefabHandler(ISnapshotterReferences references)
        {
            _references = references;

            _yingletInstance = GameObject.Instantiate(_references.YingletPrefab);
            foreach (var snapshottable in _yingletInstance.GetComponentsInChildren<ISnapshottable>())
            {
                snapshottable.PrepareForSnapshot();
            }
            SetLayerRecursively(_yingletInstance, _references.LayerIndex);
        }

        static void SetLayerRecursively(GameObject obj, int layer)
        {
            obj.layer = layer;

            foreach (Transform child in obj.transform)
            {
                SetLayerRecursively(child.gameObject, layer);
            }
        }

        public void Dispose()
        {
            if (_yingletInstance != null && _references.CleanupObjects)
            {
                GameObject.DestroyImmediate(_yingletInstance);
            }
        }
    }
}
