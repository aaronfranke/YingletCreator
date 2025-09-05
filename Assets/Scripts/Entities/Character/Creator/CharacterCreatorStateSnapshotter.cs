using UnityEngine;

namespace Character.Creator
{
	public sealed class CharacterCreatorStateSnapshot
	{
		public CharacterCreatorStateSnapshot(string action, CachedYingletReference selected, SerializableCustomizationData serializedData)
		{
			Action = action;
			Selected = selected;
			SerializedData = serializedData;
		}
		public string Action { get; }
		public CachedYingletReference Selected { get; }
		public SerializableCustomizationData SerializedData { get; }
	}

	public interface ICharacterCreatorStateSnapshotter
	{
		CharacterCreatorStateSnapshot GetStateSnapshot(string action);
		void RestoreStateSnapshot(CharacterCreatorStateSnapshot snapshot);
	}


	internal class CharacterCreatorStateSnapshotter : MonoBehaviour, ICharacterCreatorStateSnapshotter
	{
		private ICustomizationSelection _selection;

		private void Awake()
		{
			_selection = this.GetComponent<ICustomizationSelection>();
		}

		public CharacterCreatorStateSnapshot GetStateSnapshot(string action)
		{
			return new CharacterCreatorStateSnapshot(action, _selection.Selected, null);
		}

		public void RestoreStateSnapshot(CharacterCreatorStateSnapshot snapshot)
		{
			_selection.Selected = snapshot.Selected;
		}
	}
}
