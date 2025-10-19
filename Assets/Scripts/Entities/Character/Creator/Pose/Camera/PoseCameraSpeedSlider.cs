using Reactivity;
using UnityEngine.UI;

public class PoseCameraSpeedSlider : ReactiveBehaviour
{
	private IPoseCameraData _camData;
	private Slider _slider;

	void Start()
	{
		_camData = GetComponentInParent<IPoseCameraData>();


		_slider = this.GetComponentInChildren<Slider>();
		_slider.onValueChanged.AddListener(Slider_OnValueChanged);
		AddReflector(ReflectSliderValue);
	}

	private new void OnDestroy()
	{
		base.OnDestroy();
		if (_slider == null) return;
		_slider.onValueChanged.RemoveListener(Slider_OnValueChanged);
	}

	private void ReflectSliderValue()
	{
		_slider.SetValueWithoutNotify(_camData.Speed);
	}

	private void Slider_OnValueChanged(float arg0)
	{
		_camData.Speed = arg0;
	}

}