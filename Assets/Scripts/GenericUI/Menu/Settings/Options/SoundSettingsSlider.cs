using Reactivity;
using UnityEngine.UI;

public class SliderSoundSettings : ReactiveBehaviour
{
	private ISettingsManager _settingsManager;
	private Slider _slider;

	private void Awake()
	{
		_settingsManager = Singletons.GetSingleton<ISettingsManager>();
		_slider = this.GetComponentInChildren<Slider>();
		_slider.onValueChanged.AddListener(Slider_OnValueChanged);
	}

	private void Start()
	{
		AddReflector(ReflectSliderValue);
	}

	private new void OnDestroy()
	{
		base.OnDestroy();
		_slider.onValueChanged.RemoveListener(Slider_OnValueChanged);
	}

	private void ReflectSliderValue()
	{
		_slider.SetValueWithoutNotify(_settingsManager.Settings.EffectVolume);
	}


	private void Slider_OnValueChanged(float arg0)
	{
		_settingsManager.Settings.EffectVolume = arg0;
		// This is excessively serializing and writing to disk, but doing it on mouse up is a pain because you'd the pointer event is on a lower object
		_settingsManager.SaveChangesToDisk();
	}
}

