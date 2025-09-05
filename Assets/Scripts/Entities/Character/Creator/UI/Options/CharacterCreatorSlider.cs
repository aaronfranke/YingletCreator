using Character.Data;
using Reactivity;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Character.Creator.UI
{
	public class CharacterCreatorSlider : ReactiveBehaviour, IPointerUpHandler
	{
		[SerializeField] CharacterSliderId _sliderId;
		private ICustomizationSelectedDataRepository _dataRepo;
		private ICharacterCreatorUndoManager _undoManager;
		private Slider _slider;
		private bool _recordDragValue = true;

		private void Awake()
		{
			_dataRepo = this.GetComponentInParent<ICustomizationSelectedDataRepository>();
			_undoManager = this.GetComponentInParent<ICharacterCreatorUndoManager>();
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
			_slider.SetValueWithoutNotify(_dataRepo.GetSliderValue(_sliderId));
		}

		private void Slider_OnValueChanged(float arg0)
		{
			if (_recordDragValue)
			{
				// Only record to undo manager if we just started dragging this
				_undoManager.RecordState($"Adjust slider {_sliderId.name}");
				_recordDragValue = false;
			}

			_dataRepo.SetSliderValue(_sliderId, arg0);
		}

		public void OnPointerUp(PointerEventData eventData)
		{
			_recordDragValue = true;
		}
	}
}