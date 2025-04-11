using UnityEditor;
using UnityEngine;

namespace CharacterCompositor
{
	[CustomEditor(typeof(ColorGroup))]
	public class ColorGroupEditor : Editor
	{
		static Color _colorPickerColor = Color.white;

		public override void OnInspectorGUI()
		{

			ColorGroup myScript = (ColorGroup)target;

			serializedObject.Update();
			EditorGUI.BeginChangeCheck();

			// Draw default inspector UI
			DrawDefaultInspector();

			if (EditorGUI.EndChangeCheck())
			{
				serializedObject.ApplyModifiedProperties();
				UpdateGraphics();
			}

			GUILayout.BeginHorizontal();
			if (GUILayout.Button("Base to color picker"))
			{
				_colorPickerColor = myScript.BaseDefaultColor.GetColor();
			}
			if (GUILayout.Button("Shade to color picker"))
			{
				_colorPickerColor = myScript.ShadeDefaultColor.GetColor();
			}
			GUILayout.EndHorizontal();
			_colorPickerColor = EditorGUILayout.ColorField("Color Picker", _colorPickerColor);
			GUILayout.BeginHorizontal();
			if (GUILayout.Button("Color picker to base"))
			{
				Undo.RecordObject(myScript, "Changed Selected Color");
				((SerializableColorizeValues)myScript.BaseDefaultColor).Set(_colorPickerColor);
				EditorUtility.SetDirty(myScript);
				UpdateGraphics();
			}
			if (GUILayout.Button("Color picker to shade"))
			{
				Undo.RecordObject(myScript, "Changed Selected Color");
				((SerializableColorizeValues)myScript.ShadeDefaultColor).Set(_colorPickerColor);
				EditorUtility.SetDirty(myScript);
				UpdateGraphics();
			}
			GUILayout.EndHorizontal();
		}

		static void UpdateGraphics()
		{

			var composited = GameObject.FindObjectsByType<CompositedYinglet>(FindObjectsSortMode.None);
			foreach (var c in composited)
			{
				c.UpdateColorGroup();
			}
		}
	}
}