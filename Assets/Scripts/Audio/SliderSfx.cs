using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(DragSfx))]
public class SliderSfx : MonoBehaviour
{
	private Slider _slider;
	private IDragSfx _dragSfx;
	private float _previousValue = 0.5f;

	private void Awake()
	{

		_slider = this.GetComponent<Slider>();
		_dragSfx = this.GetComponent<IDragSfx>();

		_slider.onValueChanged.AddListener(Slider_OnValueChanged);
	}

	private void Slider_OnValueChanged(float value)
	{
		if (EventSystem.current.currentSelectedGameObject != _slider.gameObject)
		{
			return;
		}

		float delta = Mathf.Abs(value - _previousValue);
		_dragSfx.Change(delta);
		_previousValue = value;

	}

	private void OnDestroy()
	{
		_slider.onValueChanged.RemoveListener(Slider_OnValueChanged);
	}
}
