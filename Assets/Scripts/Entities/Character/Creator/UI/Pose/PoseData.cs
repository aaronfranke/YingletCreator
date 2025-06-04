using Reactivity;
using System.Collections.Generic;
using UnityEngine;

namespace Character.Creator.UI
{

	public interface IPoseData
	{
		IReadOnlyDictionary<CachedYingletReference, object> Data { get; }
		void ToggleYing(CachedYingletReference ying);
	}
	internal class PoseData : MonoBehaviour, IPoseData
	{
		ObservableDict<CachedYingletReference, object> _data = new();
		public IReadOnlyDictionary<CachedYingletReference, object> Data => _data;

		public void ToggleYing(CachedYingletReference ying)
		{
			if (_data.ContainsKey(ying))
			{
				_data.Remove(ying);
			}
			else
			{
				_data.Add(ying, new());
			}
		}
	}
}
