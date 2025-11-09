using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

public class PopulateDefaultResourceTableOnBuild : IPreprocessBuildWithReport
{
	public int callbackOrder => 0;

	public void OnPreprocessBuild(BuildReport report)
	{
		string[] guids = AssetDatabase.FindAssets("t:DefaultResourceLookupTable");
		if (guids.Length == 0)
		{
			throw new System.Exception("No DefaultResourceLookupTable asset");
		}
		if (guids.Length > 1)
		{
			throw new System.Exception("Multiple DefaultResourceLookupTable assets");
		}

		string path = AssetDatabase.GUIDToAssetPath(guids[0]);
		var table = AssetDatabase.LoadAssetAtPath<DefaultResourceLookupTable>(path);

		DefaultResourceLookupTableEditor.EditorPopulateDefaultTable(table);
	}
}