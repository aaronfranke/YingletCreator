using Character.Creator.UI;
using UnityEngine;

namespace Character.Creator.Pose
{
	public class ClearPoseDataOnClipboardSelectionChange : MonoBehaviour
	{

		private IInPoseModeChecker _inPoseMode;
		private IPoseData _poseData;

		void Start()
		{
			_poseData = this.GetComponentInParent<IPoseData>();
			_inPoseMode = this.GetCharacterCreatorComponent<IInPoseModeChecker>();
			_inPoseMode.InPoseMode.OnChanged += Selection_OnChanged;
		}

		private void OnDestroy()
		{
			_inPoseMode.InPoseMode.OnChanged -= Selection_OnChanged;
		}

		private void Selection_OnChanged(bool from, bool to)
		{
			if (from == false) return;

			_poseData.Clear();
		}
	}
}