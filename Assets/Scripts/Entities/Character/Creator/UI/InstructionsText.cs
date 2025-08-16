using Reactivity;
using System;
using TMPro;
using UnityEngine;

namespace Character.Creator.UI
{
	enum InstructionsType
	{
		Main,
		Pose_NoEditing,
		Pose_EditingYing,
		Pose_CameraMode
	}

	public class InstructionsText : ReactiveBehaviour
	{
		[SerializeField][TextArea] string _main;
		[SerializeField][TextArea] string _poseNoEditing;
		[SerializeField][TextArea] string _poseEditingYing;
		[SerializeField][TextArea] string _poseCameraMode;
		private ICharacterCreatorVisibilityControl _visibilityControl;
		private IInPoseModeChecker _inPoseMode;
		private IPoseData _poseData;
		private TMP_Text _text;
		Computed<InstructionsType> _instructionsType;

		void Start()
		{
			_visibilityControl = this.GetComponentInParent<ICharacterCreatorVisibilityControl>();
			_inPoseMode = this.GetCharacterCreatorComponent<IInPoseModeChecker>();
			_poseData = this.GetComponentInParent<IPoseData>();
			_text = this.GetComponent<TMPro.TMP_Text>();
			_instructionsType = CreateComputed(ComputeInstructionType);
			AddReflector(ReflectText);
		}


		private InstructionsType ComputeInstructionType()
		{
			// Are we in no-UI camera mode?
			if (!_visibilityControl.IsVisible.Val)
			{
				return InstructionsType.Pose_CameraMode;
			}

			// Are we not in pose mode?
			bool isPose = _inPoseMode.InPoseMode.Val;
			if (!isPose)
			{
				return InstructionsType.Main;
			}

			// We're in pose mode. Are we editing a ying?
			bool isEditing = _poseData.CurrentlyEditing != null;
			return isEditing
				? InstructionsType.Pose_EditingYing
				: InstructionsType.Pose_NoEditing;
		}
		private void ReflectText()
		{
			_text.text = _instructionsType.Val switch
			{
				InstructionsType.Main => _main,
				InstructionsType.Pose_NoEditing => _poseNoEditing,
				InstructionsType.Pose_EditingYing => _poseEditingYing,
				InstructionsType.Pose_CameraMode => _poseCameraMode,
				_ => throw new ArgumentOutOfRangeException()
			};
		}
	}
}