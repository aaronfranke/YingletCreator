using Character.Data;
using System.Linq;

public static class ModDefinitionExtensionMethods
{
	public struct ModTypeCounts
	{
		public int Presets;
		public int Toggles;
		public int Poses;
	}
	public static ModTypeCounts CountAssetTypes(this ModDefinition mod)
	{
		var counts = new ModTypeCounts();
		counts.Presets = mod.PresetYings.Count();
		foreach (var resource in mod.Table.Resources)
		{
			var obj = resource.Object;
			if (obj is CharacterToggleId)
			{
				counts.Toggles += 1;
			}
			else if (obj is PoseId)
			{
				counts.Poses += 1;
			}
		}
		return counts;
	}
}
