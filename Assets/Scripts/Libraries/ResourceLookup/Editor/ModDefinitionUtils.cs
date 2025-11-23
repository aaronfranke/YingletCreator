using System.Linq;

public static class ModDefinitionUtils
{
	public static ModDefinition GetBuiltinModDefinition()
	{
		var modDefinitions = ObjectExtensionMethods.LoadAllAssets<ModDefinition>()
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
		return modDefinitions.Single();
	}
}
