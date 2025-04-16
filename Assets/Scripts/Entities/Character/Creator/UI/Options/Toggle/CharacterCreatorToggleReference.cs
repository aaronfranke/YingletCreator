using Character.Data;
using UnityEngine;

namespace Character.Creator.UI
{
	public interface ICharacterCreatorToggleReference
	{
		CharacterToggleId ToggleId { get; set; }
	}
	public class CharacterCreatorToggleReference : MonoBehaviour, ICharacterCreatorToggleReference
	{
		public CharacterToggleId ToggleId { get; set; }
	}
}