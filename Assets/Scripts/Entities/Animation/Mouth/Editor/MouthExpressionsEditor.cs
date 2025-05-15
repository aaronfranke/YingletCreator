using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MouthExpressions))]
public class MouthExpressionsEditor : Editor
{
	public override void OnInspectorGUI()
	{
		// Draw the default inspector
		DrawDefaultInspector();

		// Add a space then a button
		GUILayout.Space(10);

		if (GUILayout.Button("Open Mouth"))
		{
			MouthExpressions mouth = (MouthExpressions)target;
			mouth.EditorOpen();
		}
	}
}