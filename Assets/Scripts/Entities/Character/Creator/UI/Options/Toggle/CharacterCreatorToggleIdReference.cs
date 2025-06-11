using Character.Data;
using UnityEngine;

namespace Character.Creator.UI
{
	public interface ICharacterCreatorToggleReference
	{
		string DisplayName { get; }
		Sprite Sprite { get; }
	}

	public interface ICharacterCreatorToggleIdReference : ICharacterCreatorToggleReference
	{
		CharacterToggleId ToggleId { get; set; }
	}
	public class CharacterCreatorToggleIdReference : MonoBehaviour, ICharacterCreatorToggleIdReference
	{
		public CharacterToggleId ToggleId { get; set; }

		public string DisplayName => ToggleId.DisplayName;

		public Sprite Sprite => ToggleId.Preview.Sprite;
	}
}