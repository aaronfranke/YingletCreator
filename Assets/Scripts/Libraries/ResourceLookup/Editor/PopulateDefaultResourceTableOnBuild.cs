using System.Linq;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

public class PopulateDefaultResourceTableOnBuild : IPreprocessBuildWithReport
{
	public int callbackOrder => 0;

	public void OnPreprocessBuild(BuildReport report)
	{
		var guids = AssetDatabase.FindAssets("t:DefaultResourceLookupTable");
		var modDefinitions = guids
			.Select(guid =>
			{
				var path = AssetDatabase.GUIDToAssetPath(guid);
				var modDefinition = AssetDatabase.LoadAssetAtPath<ModDefinition>(path);
				return modDefinition;
			})
			.Where(modDefinition => modDefinition.IsBuiltInMod)
			.ToArray();
		if (modDefinitions.Length == 0)
		{
			throw new System.Exception("No default mod asset");
		}
		if (modDefinitions.Length > 1)
		{
			throw new System.Exception("Multiple default mod assets");
		}

		var modDefinition = modDefinitions.Single();
		ResourceTablePopulationUtils.PopulateLookupTable(modDefinition);

	}
}