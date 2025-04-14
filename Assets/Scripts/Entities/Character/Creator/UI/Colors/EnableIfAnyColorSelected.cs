using Reactivity;
using UnityEngine;

namespace Character.Creator.UI
{
	public class EnableIfAnyColorSelected : ReactiveBehaviour
	{
		[SerializeField] bool _invert;
		private IColorActiveSelection _activeSelection;

		private void Awake()
		{
			_activeSelection = this.GetComponentInParent<IColorActiveSelection>();
			AddReflector(Reflect);
		}

		private void Reflect()
		{
			this.gameObject.SetActive(_activeSelection.AnySelected != _invert);
		}
	}
}