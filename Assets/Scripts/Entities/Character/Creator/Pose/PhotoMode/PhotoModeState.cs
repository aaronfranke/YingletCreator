using Reactivity;
using UnityEngine;

namespace Character.Creator.UI
{
	public interface IPhotoModeState
	{
		IReadOnlyObservable<bool> IsInPhotoMode { get; }
		void Toggle();
	}
	public class PhotoModeState : MonoBehaviour, IPhotoModeState
	{
		Observable<bool> _isInPhotoMode = new Observable<bool>(false);

		public IReadOnlyObservable<bool> IsInPhotoMode => _isInPhotoMode;
		public void Toggle()
		{
			_isInPhotoMode.Val = !_isInPhotoMode.Val;
		}
	}
}