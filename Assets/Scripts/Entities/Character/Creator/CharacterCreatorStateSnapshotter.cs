using UnityEngine;

namespace Character.Creator
{
	public sealed class CharacterCreatorStateSnapshot
	{
		public CharacterCreatorStateSnapshot(string action)
		{
			Action = action;
		}
		public string Action { get; }
	}

	public interface ICharacterCreatorStateSnapshotter
	{
		CharacterCreatorStateSnapshot GetStateSnapshot(string action);
		void RestoreStateSnapshot(CharacterCreatorStateSnapshot snapshot);
	}


	internal class CharacterCreatorStateSnapshotter : MonoBehaviour, ICharacterCreatorStateSnapshotter
	{
		public CharacterCreatorStateSnapshot GetStateSnapshot(string action)
		{
			return new CharacterCreatorStateSnapshot(action);
		}

		public void RestoreStateSnapshot(CharacterCreatorStateSnapshot snapshot)
		{
		}
	}
}
