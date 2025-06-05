using System;
using UnityEngine;

public static class GameObjectExtensionMethods
{
	public static IDisposable TemporarilyDisable(this GameObject prefab)
	{
		prefab.SetActive(false);
		return new ReEnableOnDispose(prefab);
	}

	sealed class ReEnableOnDispose : IDisposable
	{
		private GameObject _prefab;
		public ReEnableOnDispose(GameObject prefab)
		{
			_prefab = prefab;
		}
		public void Dispose()
		{
			_prefab.SetActive(true);
		}
	}
}
