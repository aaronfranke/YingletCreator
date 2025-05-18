using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MouthComposite))]
public class MouthCompositeEditor : Editor
{
	public override void OnInspectorGUI()
	{
		// Draw the default inspector first
		base.OnInspectorGUI();

		// Add some spacing
		EditorGUILayout.Space();

		// Draw the "Composite" button
		if (GUILayout.Button("Composite"))
		{
			MouthComposite mouthComposite = (MouthComposite)target;
		}
	}
}
