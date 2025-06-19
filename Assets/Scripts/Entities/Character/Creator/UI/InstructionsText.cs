using Character.Creator.UI;
using Reactivity;
using System;
using TMPro;
using UnityEngine;

enum InstructionsType
{
	Main,
	Pose_NoEditing,
	Pose_EditingYing
}

public class InstructionsText : ReactiveBehaviour
{
	[SerializeField][TextArea] string _main;
	[SerializeField][TextArea] string _poseNoEditing;
	[SerializeField][TextArea] string _poseEditingYing;

	private IClipboardSelection _clipboardSelection;
	private IPoseData _poseData;
	private TMP_Text _text;
	Computed<InstructionsType> _instructionsType;

	void Start()
	{
		_clipboardSelection = this.GetCharacterCreatorComponent<IClipboardSelection>();
		_poseData = this.GetComponentInParent<IPoseData>();
		_text = this.GetComponent<TMPro.TMP_Text>();
		_instructionsType = CreateComputed(ComputeInstructionType);
		AddReflector(ReflectText);
	}


	private InstructionsType ComputeInstructionType()
	{
		bool isPose = _clipboardSelection.Selection.Val == ClipboardSelectionType.Pose;
		if (!isPose)
		{
			return InstructionsType.Main;
		}
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
			_ => throw new ArgumentOutOfRangeException()
		};
	}
}
