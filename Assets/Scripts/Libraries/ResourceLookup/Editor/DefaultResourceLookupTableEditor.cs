using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DefaultResourceLookupTable))]
public class DefaultResourceLookupTableEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		DefaultResourceLookupTable lookup = (DefaultResourceLookupTable)target;

		if (GUILayout.Button("Manually Update Table"))
		{

			lookup.Table = ResourceTablePopulationUtils.PopulateLookupTable("Assets/ScriptableObjects", false);

			// Mark modified and save
			EditorUtility.SetDirty(lookup);
			AssetDatabase.SaveAssets();
		}
	}
}
