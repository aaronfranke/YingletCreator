using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Character.Creator
{
	public interface ICharacterCreatorUndoManager
	{
		/// <summary>
		/// Consumers of this will typically be the actual UI buttons and things
		/// This is because the DataRepository doesn't know the context of the action
		/// i.e.: State should only be recorded when a slider starts to be dragged
		/// </summary>
		void RecordState(string action);

		void TryUndo();
		void TryRedo();

		/// <summary>
		/// Event fired when an undo was applied with a description of the operation
		/// </summary>
		event Action<string> UndoApplied;

		/// <summary>
		/// Event fired when an undo was attempted but there was nothing to undo
		/// </summary>
		event Action NothingToUndo;

		event Action<string> RedoApplied;
		event Action NothingToRedo;
	}

	public class CharacterCreatorUndoManager : MonoBehaviour, ICharacterCreatorUndoManager
	{
		private ICharacterCreatorStateSnapshotter _stateSnapshotter;

		List<CharacterCreatorStateSnapshot> _undoStack = new(); // Not actually a stack because we need to remove things from the front

		public event Action<string> UndoApplied;
		public event Action NothingToUndo;
		public event Action<string> RedoApplied;
		public event Action NothingToRedo;

		private void Awake()
		{
			_stateSnapshotter = this.GetComponent<ICharacterCreatorStateSnapshotter>();
		}

		public void RecordState(string action)
		{
			var undoState = _stateSnapshotter.GetStateSnapshot(action);
			_undoStack.Add(undoState);
		}

		public void TryRedo()
		{
		}

		public void TryUndo()
		{
			// Check if we even have anything
			if (!_undoStack.Any())
			{
				NothingToUndo?.Invoke();
				return;
			}

			var state = _undoStack.Last();
			_undoStack.RemoveAt(_undoStack.Count - 1);
			_stateSnapshotter.RestoreStateSnapshot(state);
			UndoApplied?.Invoke(state.Action);
		}
	}
}