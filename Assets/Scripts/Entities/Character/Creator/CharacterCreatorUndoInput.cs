
using Character.Creator.UI;
using UnityEngine;

namespace Character.Creator
{

	public class CharacterCreatorUndoInput : MonoBehaviour
	{
		private ICharacterCreatorUndoManager _undoManager;
		private IInPoseModeChecker _inPoseMode;

		private void Awake()
		{
			_undoManager = GetComponentInParent<ICharacterCreatorUndoManager>();
			_inPoseMode = this.GetCharacterCreatorComponent<IInPoseModeChecker>();
		}

		private void Update()
		{
			if (!Input.GetKey(KeyCode.LeftControl)) return; // Need to hold ctrl
			if (_inPoseMode.InPoseMode.Val) return; // Need to not be in pose mode

			if (Input.GetKeyDown(KeyCode.Z))
			{
				_undoManager.TryUndo();
			}
			else if (Input.GetKeyDown(KeyCode.Y))
			{
				_undoManager.TryRedo();
			}
		}
	}
}