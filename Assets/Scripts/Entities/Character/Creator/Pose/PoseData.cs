using Character.Creator;
using Reactivity;
using System.Collections.Generic;
using UnityEngine;

public interface IPoseData
{

	/// <summary>
	/// The pose data for all the yings that have been selected by the user in the UI
	/// </summary>
	IReadOnlyDictionary<CachedYingletReference, object> Data { get; }

	/// <summary>
	/// The individual ying that has been last been clicked by the user in the scene view
	/// Can be null
	/// </summary>
	PoseYing CurrentlyEditing { get; set; }

	/// <summary>
	/// Adds or removes the given ying to the pose data
	/// </summary>
	void ToggleYing(CachedYingletReference ying);
}
internal class PoseData : MonoBehaviour, IPoseData
{
	Observable<PoseYing> _currentlyEditing = new();
	ObservableDict<CachedYingletReference, object> _data = new();

	public IReadOnlyDictionary<CachedYingletReference, object> Data => _data;
	public PoseYing CurrentlyEditing { get => _currentlyEditing.Val; set => _currentlyEditing.Val = value; }

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

public sealed class PoseYing
{
	public CachedYingletReference Reference { get; }
	public GameObject GameObject { get; }
	public PoseYing(CachedYingletReference reference, GameObject gameObject)
	{
		Reference = reference;
		GameObject = gameObject;
	}
}