using UnityEngine;

namespace Character.Creator.UI
{
	public class ColorCopyPastingToasts : MonoBehaviour
	{
		private IToastDisplay _toastDisplay;
		private IColorCopyPasting _copyPasting;

		private void Awake()
		{
			_toastDisplay = Singletons.GetSingleton<IToastDisplay>();
			_copyPasting = this.GetComponent<IColorCopyPasting>();
			_copyPasting.Copied += CopyPasting_Copied;
			_copyPasting.Pasted += CopyPasting_Pasted;
		}

		private void OnDestroy()
		{
			_copyPasting.Copied -= CopyPasting_Copied;
			_copyPasting.Pasted -= CopyPasting_Pasted;
		}
		private void CopyPasting_Copied()
		{
			_toastDisplay.Show("Color copied");
		}
		private void CopyPasting_Pasted()
		{
			_toastDisplay.Show("Color pasted");
		}
	}
}