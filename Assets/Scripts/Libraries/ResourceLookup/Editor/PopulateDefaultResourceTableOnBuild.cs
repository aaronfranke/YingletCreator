using UnityEditor.Build;
using UnityEditor.Build.Reporting;

public class PopulateDefaultResourceTableOnBuild : IPreprocessBuildWithReport
{
	public int callbackOrder => 0;

	public void OnPreprocessBuild(BuildReport report)
	{
		var modDefinition = ModDefinitionUtils.GetBuiltinModDefinition();
		ResourceTablePopulationUtils.PopulateLookupTable(modDefinition);
	}
}