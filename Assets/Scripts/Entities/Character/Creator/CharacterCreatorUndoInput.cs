
using UnityEngine;

namespace Character.Creator
{

	public class CharacterCreatorUndoInput : MonoBehaviour
	{
		private ICharacterCreatorUndoManager _undoManager;

		private void Awake()
		{
			_undoManager = GetComponentInParent<ICharacterCreatorUndoManager>();
		}

		private void Update()
		{
			if (!Input.GetKey(KeyCode.LeftControl)) return;

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