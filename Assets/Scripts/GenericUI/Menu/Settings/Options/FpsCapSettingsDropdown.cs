using Reactivity;
using UnityEngine.UI;

internal class FpsCapSettingsDropdown : MenuSettingsDropdown<FpsCap>
{
	protected override FpsCap Value 
	{ 
		get => _settingsManager.Settings.FpsCap; 
		set => _settingsManager.Settings.FpsCap = value; 
	}

	protected override MenuSettingsDropdownOption[] GetAllOptions()
	{
		return new MenuSettingsDropdownOption[]
		{
			new MenuSettingsDropdownOption("30 FPS", FpsCap.Fps30),
			new MenuSettingsDropdownOption("60 FPS", FpsCap.Fps60),
			new MenuSettingsDropdownOption("120 FPS", FpsCap.Fps120),
			new MenuSettingsDropdownOption("144 FPS", FpsCap.Fps144),
			new MenuSettingsDropdownOption("240 FPS", FpsCap.Fps240),
			new MenuSettingsDropdownOption("Unlimited", FpsCap.Unlimited)
		};
	}
}
