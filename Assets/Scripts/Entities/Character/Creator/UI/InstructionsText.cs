using Character.Creator.UI;
using Reactivity;
using System;
using TMPro;
using UnityEngine;

enum InstructionsType
{
	Main,
	CameraPose
}

public class InstructionsText : ReactiveBehaviour
{
	[SerializeField][TextArea] string _mainInstructions;
	[SerializeField][TextArea] string _cameraPoseInstructions;
	private IClipboardSelection _clipboardSelection;
	private TMP_Text _text;
	Computed<InstructionsType> _instructionsType;

	void Start()
	{
		_clipboardSelection = this.GetCharacterCreatorComponent<IClipboardSelection>();
		_text = this.GetComponent<TMPro.TMP_Text>();
		_instructionsType = CreateComputed(ComputeInstructionType);
		AddReflector(ReflectText);
	}


	private InstructionsType ComputeInstructionType()
	{
		bool isPose = _clipboardSelection.Selection.Val == ClipboardSelectionType.Pose;
		return isPose ? InstructionsType.CameraPose : InstructionsType.Main;
	}
	private void ReflectText()
	{
		_text.text = _instructionsType.Val switch
		{
			InstructionsType.Main => _mainInstructions,
			InstructionsType.CameraPose => _cameraPoseInstructions,
			_ => throw new ArgumentOutOfRangeException()
		};
	}
}
