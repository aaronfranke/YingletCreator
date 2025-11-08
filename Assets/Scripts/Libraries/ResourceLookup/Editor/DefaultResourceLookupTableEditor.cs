using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

[CustomEditor(typeof(DefaultResourceLookupTable))]
public class DefaultResourceLookupTableEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		DefaultResourceLookupTable lookup = (DefaultResourceLookupTable)target;

		if (GUILayout.Button("Manually Update Table"))
		{
			PopulateLookup(lookup);
		}
	}

	private void PopulateLookup(DefaultResourceLookupTable lookup)
	{
		if (lookup.Table == null)
		{
			lookup.Table = new ResourceLookupTable();
		}

		// Find all ScriptableObject assets in project
		// I originally thought of just searching for the ones being used and working my way down but:
		// - Some assets are referenced by things in the UI
		// - Some are provided for modders (despite not being used itself)
		string[] guids = AssetDatabase.FindAssets("t:ScriptableObject", new[] { "Assets/ScriptableObjects" });
		var resources = new ResourcePair[guids.Length];

		Dictionary<string, Object> table = new Dictionary<string, Object>();

		foreach (var guid in guids)
		{
			LoadObjectAndAddRecursively(guid);
		}

		lookup.Table.Resources = table
			.Select(kvp => new ResourcePair(kvp.Key, kvp.Value))
			.ToArray();

		// Mark modified and save
		EditorUtility.SetDirty(lookup);
		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();

		Debug.Log($"Populated lookup table with {table.Count} objects.");

		void LoadObjectAndAddRecursively(string guid)
		{
			if (string.IsNullOrWhiteSpace(guid))
			{
				return;
			}
			if (table.ContainsKey(guid))
			{
				return; // May have already added this if it was a reference from something else
			}

			string path = AssetDatabase.GUIDToAssetPath(guid);
			// TODO: filter out certain paths here when adding for ModDefinitions

			Object obj = AssetDatabase.LoadAssetAtPath<Object>(path);
			table.Add(guid, obj);

			var fields = GetAssetReferenceFields(obj);
			foreach (var field in fields)
			{
				LoadObjectAndAddRecursively(field.AssetGUID);
			}
		}
	}
	public static IEnumerable<AssetReference> GetAssetReferenceFields(UnityEngine.Object target)
	{
		var results = new List<AssetReference>();
		var type = target.GetType();

		var flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

		while (type != null && type != typeof(UnityEngine.Object))
		{
			var fields = type.GetFields(flags | BindingFlags.DeclaredOnly);
			foreach (var field in fields)
			{
				if (!typeof(AssetReference).IsAssignableFrom(field.FieldType))
				{
					continue;
				}

				var instance = field.GetValue(target) as AssetReference;
				if (instance != null)
				{
					results.Add(instance);
				}
			}

			type = type.BaseType;
		}

		return results;
	}
}
