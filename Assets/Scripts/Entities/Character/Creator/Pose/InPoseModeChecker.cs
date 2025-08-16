using Reactivity;
using UnityEngine;

namespace Character.Creator.UI
{
	public interface IInPoseModeChecker
	{
		IReadOnlyObservable<bool> InPoseMode { get; }

	}

	public class InPoseModeChecker : ReactiveBehaviour, IInPoseModeChecker
	{
		[SerializeField] ClipboardSelectionType _poseModeType;

		private IClipboardSelection _clipboardSelection;
		Computed<bool> _inPoseMode;

		public IReadOnlyObservable<bool> InPoseMode => _inPoseMode;

		private void Awake()
		{
			_clipboardSelection = this.GetComponent<IClipboardSelection>();
			_inPoseMode = CreateComputed(ComputeInPoseMode);

		}

		private bool ComputeInPoseMode()
		{
			return _clipboardSelection.Selection.Val == _poseModeType;
		}
	}
}
