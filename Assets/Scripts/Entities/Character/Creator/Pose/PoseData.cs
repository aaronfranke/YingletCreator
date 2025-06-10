using Character.Creator;
using Reactivity;
using System.Collections.Generic;
using UnityEngine;

public interface IPoseData
{
	/// <summary>
	/// The pose data for all the yings that have been selected by the user in the UI
	/// </summary>
	IReadOnlyDictionary<CachedYingletReference, IYingPoseData> Data { get; }

	/// <summary>
	/// The individual ying that has been last been clicked by the user in the scene view
	/// Can be null
	/// </summary>
	PoseYing CurrentlyEditing { get; set; }

	/// <summary>
	/// If the ying selected for editing an eventh selection
	/// This is used to swap between ying pose data pages
	/// </summary>
	bool EditingEven { get; }

	/// <summary>
	/// Adds or removes the given ying to the pose data
	/// </summary>
	void ToggleYing(CachedYingletReference ying);

	/// <summary>
	/// Clear all data out
	/// </summary>
	void Clear();
}
internal class PoseData : MonoBehaviour, IPoseData
{
	Observable<PoseYing> _currentlyEditing = new();
	Observable<bool> _editingEven = new(true);
	ObservableDict<CachedYingletReference, IYingPoseData> _data = new();

	public IReadOnlyDictionary<CachedYingletReference, IYingPoseData> Data => _data;
	public PoseYing CurrentlyEditing
	{
		get => _currentlyEditing.Val;
		set
		{
			if (_currentlyEditing.Val == value) return;
			_currentlyEditing.Val = value;
			_editingEven.Val = !_editingEven.Val; // Toggle this
		}
	}
	public bool EditingEven => _editingEven.Val;

	public void Clear()
	{
		_currentlyEditing.Val = null;
		_data.Clear();
	}

	public void ToggleYing(CachedYingletReference ying)
	{
		if (_data.ContainsKey(ying))
		{
			if (CurrentlyEditing?.Reference == ying)
			{
				CurrentlyEditing = null;
			}
			_data.Remove(ying);
		}
		else
		{
			_data.Add(ying, new YingPoseData());
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