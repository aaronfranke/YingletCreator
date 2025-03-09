using UnityEditor;
using UnityEngine;

namespace CharacterCompositor
{
	[CustomEditor(typeof(ColorGroup))]
	public class ColorGroupEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			serializedObject.Update();
			EditorGUI.BeginChangeCheck();

			// Draw default inspector UI
			DrawDefaultInspector();

			if (EditorGUI.EndChangeCheck())
			{
				serializedObject.ApplyModifiedProperties();

				var composited = GameObject.FindObjectsByType<CompositedYinglet>(FindObjectsSortMode.None);
				foreach (var c in composited)
				{
					c.UpdateColorGroup();
				}
			}
		}
	}
}