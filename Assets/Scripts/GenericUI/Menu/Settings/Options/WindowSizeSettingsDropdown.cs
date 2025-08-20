using UnityEngine.Assertions;

internal class WindowSizeSettingsDropdown : MenuSettingsDropdown<WindowSize>
{
	protected override WindowSize Value
	{
		get => _settingsManager.Settings.WindowSize;
		set => _settingsManager.Settings.WindowSize = value;
	}

	protected override MenuSettingsDropdownOption[] GetAllOptions()
	{
		var options = new[]
		{
			new MenuSettingsDropdownOption("1024×768", WindowSize.Resolution1024x768),
			new MenuSettingsDropdownOption("1280×720", WindowSize.Resolution1280x720),
			new MenuSettingsDropdownOption("1280×800", WindowSize.Resolution1280x800),
			new MenuSettingsDropdownOption("1280×960", WindowSize.Resolution1280x960),
			new MenuSettingsDropdownOption("1440×900", WindowSize.Resolution1440x900),
			new MenuSettingsDropdownOption("1600×900", WindowSize.Resolution1600x900),
			new MenuSettingsDropdownOption("1680×1050", WindowSize.Resolution1680x1050),
			new MenuSettingsDropdownOption("1920×1080", WindowSize.Resolution1920x1080),
			new MenuSettingsDropdownOption("1920×1200", WindowSize.Resolution1920x1200),
			new MenuSettingsDropdownOption("2560×1080", WindowSize.Resolution2560x1080),
			new MenuSettingsDropdownOption("2560×1440", WindowSize.Resolution2560x1440),
			new MenuSettingsDropdownOption("3440×1440", WindowSize.Resolution3440x1440),
			new MenuSettingsDropdownOption("3840×2160", WindowSize.Resolution3840x2160)
		};
		Assert.AreEqual(options.Length, System.Enum.GetValues(typeof(WindowSize)).Length, "Mismatch between defined options and enum values.");
		return options;
	}
}
