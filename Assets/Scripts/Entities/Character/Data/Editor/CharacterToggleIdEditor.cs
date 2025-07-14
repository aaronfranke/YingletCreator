using Character.Data;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

// This tells Unity to use this editor for the CharacterToggleID type
[CustomEditor(typeof(CharacterToggleId)), CanEditMultipleObjects]
public class CharacterToggleIDEditor : Editor
{
	private List<CharacterToggleId> _cachedToggleIds = new();
	private Vector2 _scrollPos;

	private void OnEnable()
	{
		LoadAllToggleIds();
	}

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		EditorGUILayout.Space();

		if (targets.Length > 1)
		{
			return;
		}

		if (GUILayout.Button("Refresh Order View"))
		{
			LoadAllToggleIds();
		}

		DisplayOrderList();
	}

	void LoadAllToggleIds()
	{
		var toggleId = target as CharacterToggleId;
		var orderGroup = toggleId.Order.Group;
		if (toggleId.Order.Group == null) return;

		_cachedToggleIds.Clear();

		string[] guids = AssetDatabase.FindAssets("t:CharacterToggleID");
		foreach (string guid in guids)
		{
			string path = AssetDatabase.GUIDToAssetPath(guid);
			CharacterToggleId asset = AssetDatabase.LoadAssetAtPath<CharacterToggleId>(path);
			if (asset == null) continue;
			if (asset.Order.Group != orderGroup) continue;
			_cachedToggleIds.Add(asset);
		}

		_cachedToggleIds = _cachedToggleIds.OrderBy(toggle => toggle.Order.Index).ToList();
	}

	void DisplayOrderList()
	{
		if (!_cachedToggleIds.Any()) return;

		EditorGUILayout.Space();
		EditorGUILayout.LabelField("All CharacterToggleIDs in Project:", EditorStyles.boldLabel);

		_scrollPos = EditorGUILayout.BeginScrollView(_scrollPos, GUILayout.Height(200));
		foreach (var toggle in _cachedToggleIds)
		{
			EditorGUILayout.ObjectField($"({toggle.Order.Index}) {toggle.name}", toggle, typeof(CharacterToggleId), false);
		}
		EditorGUILayout.EndScrollView();
	}
}