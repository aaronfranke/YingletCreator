using Character.Data;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ModDefinition))]
public class ModDefinitionEditor : Editor
{
	SerializedProperty _modDisplayTitleProp;
	SerializedProperty _toggles;

	void OnEnable()
	{
		_modDisplayTitleProp = serializedObject.FindProperty("_modDisplayTitle");
		_toggles = serializedObject.FindProperty("_toggles");
	}

	public override void OnInspectorGUI()
	{
		serializedObject.Update();

		EditorGUILayout.LabelField("This is the title that will display in-game and on the Steam workshop:");
		EditorGUILayout.PropertyField(_modDisplayTitleProp);

		DrawHorizontalLine(Color.gray);


		EditorGUILayout.LabelField("Click the following button to gather created content in this folder directory. This includes:");
		EditorGUILayout.LabelField(" • Presets (.yingsave)");
		EditorGUILayout.LabelField(" • Toggles (CharacterToggleId)");
		EditorGUILayout.LabelField(" • Poses (PoseID)");
		EditorGUILayout.LabelField("Supporting textures, models, and ScriptableObject metadata will also be bundled.");

		var modDefinition = (ModDefinition)target;

		if (GUILayout.Button("Gather Mod Content"))
		{
			GatherModContent(modDefinition);
		}

		DrawHorizontalLine(Color.gray);

		EditorGUILayout.LabelField("Content found:");
		GUI.enabled = false;
		_toggles.isExpanded = true;
		EditorGUILayout.PropertyField(_toggles);
		GUI.enabled = true;

		serializedObject.ApplyModifiedProperties();
	}

	private static void DrawHorizontalLine(Color color, float thickness = 1f, float padding = 6f)
	{
		EditorGUILayout.Space();

		Rect rect = EditorGUILayout.GetControlRect(false, thickness + padding);
		rect.height = thickness;
		rect.y += padding / 2f;
		EditorGUI.DrawRect(rect, color);
		EditorGUILayout.Space();
	}

	static void GatherModContent(ModDefinition modDefinition)
	{
		// Find toggles under the mod's folder
		var toggles = FindToggles(modDefinition)
			.Where(t => t != null) // guard against any failed loads
			.ToArray();

		// Use a SerializedObject to modify the serialized _toggles array
		var so = new SerializedObject(modDefinition);
		var togglesProp = so.FindProperty("_toggles");

		so.Update();

		togglesProp.arraySize = toggles.Length;
		for (int i = 0; i < toggles.Length; i++)
		{
			togglesProp.GetArrayElementAtIndex(i).objectReferenceValue = toggles[i];
		}

		so.ApplyModifiedProperties();

		// Ensure the asset is marked dirty and saved to disk
		EditorUtility.SetDirty(modDefinition);
		AssetDatabase.SaveAssets();

		Debug.Log($"Gather Mod Content ran: Found and assigned {toggles.Length} toggle(s) for '{modDefinition.name}'.");
	}

	static IEnumerable<CharacterToggleId> FindToggles(ModDefinition root)
	{
		var assetPath = AssetDatabase.GetAssetPath(root);

		var folderPath = Path.GetDirectoryName(assetPath);

		var guids = AssetDatabase.FindAssets($"t:{nameof(CharacterToggleId)}", new[] { folderPath });

		var toggles = guids.Select(guid =>
		{
			var path = AssetDatabase.GUIDToAssetPath(guid);
			return AssetDatabase.LoadAssetAtPath<CharacterToggleId>(path);
		}).ToArray();

		return toggles;
	}

}