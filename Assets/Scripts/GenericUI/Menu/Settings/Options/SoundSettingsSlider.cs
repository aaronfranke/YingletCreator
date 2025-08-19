using Character.Data;
using Reactivity;
using UnityEngine;
using UnityEngine.UI;

public class SliderSoundSettings : ReactiveBehaviour
{

	[SerializeField] CharacterSliderId _sliderId;
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
	}
}

