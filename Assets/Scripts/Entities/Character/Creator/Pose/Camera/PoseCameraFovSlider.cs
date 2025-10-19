using Reactivity;
using UnityEngine.UI;

public class PoseCameraFovSlider : ReactiveBehaviour
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
		_slider.onValueChanged.RemoveListener(Slider_OnValueChanged);
	}

	private void ReflectSliderValue()
	{
		_slider.SetValueWithoutNotify(_camData.FieldOfView);
	}

	private void Slider_OnValueChanged(float arg0)
	{
		_camData.FieldOfView = arg0;
	}

}
