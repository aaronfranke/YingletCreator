using Reactivity;
using System.Linq;
using UnityEngine;

namespace Character.Creator.UI
{
	public enum ClipboardSelectionType
	{
		PresetsSaves,
		FurHair,
		EyesMouthEars,
		Clothes,
		Colors,
		Sliders,
		Background,
		Pose
	}

	public interface IClipboardSelection
	{
		Observable<ClipboardSelectionType> Selection { get; }

		void SetSelection(ClipboardSelectionType selection);
		IPage GetPageWithType(ClipboardSelectionType type);
	}

	public class ClipboardSelection : MonoBehaviour, IClipboardSelection
	{
		[SerializeField] ClipboardSelectionType _initialSelection;

		public Observable<ClipboardSelectionType> Selection { get; } = new Observable<ClipboardSelectionType>();

		void Start()
		{
			Selection.Val = _initialSelection;
		}

		public void SetSelection(ClipboardSelectionType selection)
		{
			Selection.Val = selection;
		}

		public IPage GetPageWithType(ClipboardSelectionType type)
		{
			return this.GetComponentsInChildren<IPage>(true).FirstOrDefault(page => page.GetComponent<IClipboardElementSelection>().Type == type);
		}
	}
}