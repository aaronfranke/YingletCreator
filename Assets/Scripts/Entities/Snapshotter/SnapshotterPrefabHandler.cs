using System;
using UnityEngine;

namespace Snapshotter
{
	internal sealed class SnapshotterPrefabHandler : IDisposable
	{
		private readonly ISnapshotterReferences _references;
		private readonly GameObject _yingletInstance;

		public SnapshotterPrefabHandler(ISnapshotterReferences references, SnapshotterParams sParams)
		{
			_references = references;
			using (_references.YingletPrefab.TemporarilyDisable())
			{
				_yingletInstance = GameObject.Instantiate(_references.YingletPrefab);
				_yingletInstance.GetComponent<SnapshotterDataRepository>().Setup(sParams.Data);
				_yingletInstance.SetActive(true);
			}
			foreach (var snapshottable in _yingletInstance.GetComponentsInChildren<ISnapshottableComponent>())
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
			_references.YingletPrefab.SetActive(true);
			if (_yingletInstance != null && _references.CleanupObjects)
			{
				GameObject.DestroyImmediate(_yingletInstance);
			}
		}

		public float GetYScale()
		{
			return _yingletInstance.GetComponentInChildren<IYingletHeightProvider>().YScale;
		}
	}
}
