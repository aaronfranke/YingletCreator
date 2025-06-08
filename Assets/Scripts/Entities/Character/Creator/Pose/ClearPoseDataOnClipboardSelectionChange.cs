using Character.Creator.UI;
using UnityEngine;

namespace Character.Creator.Pose
{
	public class ClearPoseDataOnClipboardSelectionChange : MonoBehaviour
	{

		private IClipboardSelection _clipboardSelection;
		private IPoseData _poseData;

		void Start()
		{
			_poseData = this.GetComponentInParent<IPoseData>();
			_clipboardSelection = this.GetCharacterCreatorComponent<IClipboardSelection>();
			_clipboardSelection.Selection.OnChanged += Selection_OnChanged;
		}

		private void OnDestroy()
		{
			_clipboardSelection.Selection.OnChanged -= Selection_OnChanged;
		}

		private void Selection_OnChanged(ClipboardSelectionType from, ClipboardSelectionType to)
		{
			if (from != ClipboardSelectionType.Pose) return;

			_poseData.Clear();
		}
	}
}