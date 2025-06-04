using Character.Creator.UI;
using Reactivity;
using UnityEngine;

namespace Character.Creator.Pose
{
	public class DisableOnClipboardSelection : ReactiveBehaviour
	{
		[SerializeField] ClipboardSelectionType _disableOnSelection = ClipboardSelectionType.PhotoMode;

		private IClipboardSelection _clipboardSelection;

		void Start()
		{
			_clipboardSelection = this.GetCharacterCreatorComponent<IClipboardSelection>();

			AddReflector(Reflect);
		}

		private void Reflect()
		{
			this.gameObject.SetActive(_clipboardSelection.Selection.Val != _disableOnSelection);
		}
	}
}