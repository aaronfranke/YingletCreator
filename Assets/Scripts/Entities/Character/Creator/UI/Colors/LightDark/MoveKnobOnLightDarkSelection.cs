using Reactivity;
using UnityEngine;

namespace Character.Creator.UI
{
	public class MoveKnobOnLightDarkSelection : ReactiveBehaviour
	{
		[SerializeField] SharedEaseSettings _easeSettings;
		private Vector3 _originalPosition;
		private Vector3 _flippedPosition;
		private ILightDarkSelection _selection;
		private Coroutine _transitionCoroutine;

		private void Awake()
		{
			_originalPosition = this.transform.localPosition;
			_flippedPosition = this.transform.localPosition;
			_flippedPosition.x = -_flippedPosition.x;
			_selection = this.GetComponentInParent<ILightDarkSelection>();
		}

		void Start()
		{
			AddReflector(Reflect);
		}

		private void Reflect()
		{
			Vector3 from = this.transform.localPosition;
			Vector3 to = _selection.Light ? _originalPosition : _flippedPosition;
			this.StartEaseCoroutine(ref _transitionCoroutine, _easeSettings, p => this.transform.localPosition = Vector3.LerpUnclamped(from, to, p));
		}
	}
}