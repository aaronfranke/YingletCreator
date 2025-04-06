using System;
using UnityEngine;

namespace Snapshotter
{
    internal sealed class SnapshotterCameraHandler : IDisposable
    {
        private readonly ISnapshotterReferences _references;
        private readonly SnapshotterParams _sParams;
        Camera _camera;

        public SnapshotterCameraHandler(ISnapshotterReferences references, SnapshotterParams sParams)
        {
            _references = references;
            _sParams = sParams;

            var go = new GameObject("SnapshotterCamera");
            _camera = go.AddComponent<Camera>();
            _camera.transform.position = _sParams.CamPos.Position;
            _camera.transform.rotation = Quaternion.Euler(_sParams.CamPos.Rotation);
        }

        public void Dispose()
        {
            if (_camera != null && _references.CleanupObjects)
            {
                GameObject.Destroy(_camera.gameObject);
            }
        }
    }
}
