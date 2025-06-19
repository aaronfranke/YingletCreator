using Reactivity;
using UnityEngine;

namespace Character.Creator.UI
{
	public interface ICharacterCreatorVisibilityControl
	{
		IReadOnlyObservable<bool> IsVisible { get; }
		void Toggle();
	}
	public class CharacterCreatorVisibilityControl : MonoBehaviour, ICharacterCreatorVisibilityControl
	{
		Observable<bool> _isVisible = new Observable<bool>(true);

		public IReadOnlyObservable<bool> IsVisible => _isVisible;
		public void Toggle()
		{
			_isVisible.Val = !_isVisible.Val;
		}
	}
}