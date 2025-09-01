using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;


class OrderableScriptableObjectGuiDisplayer<TScriptableObject, TGroup> where TScriptableObject : ScriptableObject, IOrderableScriptableObject<TGroup> where TGroup : ScriptableObject
{
	private List<TScriptableObject> _cachedObjects = new();
	private Vector2 _scrollPos;

	public void LoadAll(ScriptableObject group)
	{
		if (group == null) return;

		_cachedObjects.Clear();

		string[] guids = AssetDatabase.FindAssets("t:" + typeof(TScriptableObject).Name);
		foreach (string guid in guids)
		{
			string path = AssetDatabase.GUIDToAssetPath(guid);
			TScriptableObject asset = AssetDatabase.LoadAssetAtPath<TScriptableObject>(path);
			if (asset == null) continue;
			if (asset.Order.Group != group) continue;
			_cachedObjects.Add(asset);
		}

		_cachedObjects = _cachedObjects.OrderBy(obj => obj.Order.Index).ToList();
	}

	public void Display(ScriptableObject group)
	{
		if (GUILayout.Button("Refresh Order View"))
		{
			LoadAll(group);
		}

		if (!_cachedObjects.Any()) return;

		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Ordering in group:", EditorStyles.boldLabel);

		_scrollPos = EditorGUILayout.BeginScrollView(_scrollPos, GUILayout.Height(600));
		foreach (var obj in _cachedObjects)
		{
			EditorGUILayout.ObjectField($"({obj.Order.Index}) {obj.name}", obj, typeof(TScriptableObject), false);
		}
		EditorGUILayout.EndScrollView();
	}
}