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
            _camera.cullingMask = references.LayerMask;
            _camera.clearFlags = CameraClearFlags.Color;
            _camera.nearClipPlane = 0.01f;
            _camera.backgroundColor = Color.clear;
        }

        public void RenderTo(RenderTexture rt)
        {
            _camera.targetTexture = rt;
            _camera.Render();
            _camera.targetTexture = null;
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
