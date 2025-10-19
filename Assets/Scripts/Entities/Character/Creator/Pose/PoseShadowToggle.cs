using Reactivity;
using UnityEngine.UI;

namespace Character.Creator.UI
{
	public class PoseShadowToggle : ReactiveBehaviour
	{
		private IPageYingPoseData _data;
		private Toggle _toggle;

		private void Awake()
		{
			_data = this.GetComponentInParent<IPageYingPoseData>();
			_toggle = this.GetComponentInChildren<Toggle>();
			_toggle.onValueChanged.AddListener(Toggle_OnValueChanged);
		}
		private new void OnDestroy()
		{
			base.OnDestroy();
			_toggle.onValueChanged.RemoveListener(Toggle_OnValueChanged);
		}

		private void Toggle_OnValueChanged(bool arg0)
		{
			_data.Data.Shadow = !_data.Data.Shadow;
		}


		private void Start()
		{
			AddReflector(ReflectToggleValue);
		}

		private void ReflectToggleValue()
		{
			var data = _data.Data;
			if (data == null) return;
			_toggle.SetIsOnWithoutNotify(data.Shadow);
		}


	}
}