using Reactivity;
using System.Collections.Generic;
using System.Linq;
using TMPro;


public abstract class MenuSettingsDropdown<T> : ReactiveBehaviour where T : System.Enum
{

	protected ISettingsManager _settingsManager;
	private TMP_Dropdown _dropdown;
	private MenuSettingsDropdownOption[] _options;

	private void Awake()
	{
		_settingsManager = Singletons.GetSingleton<ISettingsManager>();
		_dropdown = this.GetComponentInChildren<TMP_Dropdown>();
		_dropdown.onValueChanged.AddListener(Dropdown_OnValueChanged);

		_options = GetAllOptions();

		PopulateDropdownOptions();
	}

	private void Start()
	{
		AddReflector(ReflectDropdownValue);
	}

	private new void OnDestroy()
	{
		base.OnDestroy();
		_dropdown.onValueChanged.RemoveListener(Dropdown_OnValueChanged);
	}

	protected abstract MenuSettingsDropdownOption[] GetAllOptions();
	protected abstract T Value { get; set; }

	private void PopulateDropdownOptions()
	{
		_dropdown.ClearOptions();

		foreach (var option in _options)
		{
			_dropdown.options.Add(new TMP_Dropdown.OptionData(option.DisplayName));
		}
	}

	private void ReflectDropdownValue()
	{
		_dropdown.SetValueWithoutNotify(GetDropdownIndexForEnumValue(Value));
	}

	private void Dropdown_OnValueChanged(int index)
	{
		Value = GetEnumValueForDropdownIndex(index);
		_settingsManager.SaveChangesToDisk();
	}

	private int GetDropdownIndexForEnumValue(T enumValue)
	{
		return _options
			.Select((option, index) => new { option, index })
			.First(x => EqualityComparer<T>.Default.Equals(x.option.EnumValue, enumValue))
			.index;
	}

	private T GetEnumValueForDropdownIndex(int index)
	{
		return _options[index].EnumValue;
	}

	protected sealed class MenuSettingsDropdownOption
	{
		public string DisplayName { get; }
		public T EnumValue { get; }
		public MenuSettingsDropdownOption(string displayName, T enumValue)
		{
			DisplayName = displayName;
			EnumValue = enumValue;
		}
	}
}
