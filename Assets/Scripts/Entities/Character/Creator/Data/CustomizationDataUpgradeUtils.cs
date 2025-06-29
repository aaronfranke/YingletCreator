using Character.Data;

namespace Character.Creator
{
	internal static class CustomizationDataUpgradeUtils
	{
		public static void UpgradeIfNeeded(ObservableCustomizationData data, int version)
		{
			// Max version number stored in <see cref="SerializableCustomizationData"/> 
			if (version <= 0)
			{
				// Added ear and whisker toggles
				var earToggle = ResourceLoader.Load<CharacterToggleId>("ba34ff2f204a7044d927022c326264b0");
				var whiskerToggle = ResourceLoader.Load<CharacterToggleId>("67335fedf8c1d5b45b5e5d7f6781a70f");
				data.ToggleData.Toggles.Add(earToggle);
				data.ToggleData.Toggles.Add(whiskerToggle);
			}
			if (version <= 1)
			{
				// Add antenna toggle
				var antennaToggle = ResourceLoader.Load<CharacterToggleId>("8cba46f50096a834fb85f242c51bb822");
				data.ToggleData.Toggles.Add(antennaToggle);
			}
		}
	}
}
