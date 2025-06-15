using Character.Data;
using UnityEngine;

namespace Character.Creator.UI
{
	public interface ICharacterCreatorTogglePoseReference : ICharacterCreatorToggleReference
	{
		PoseId PoseId { get; set; }
	}
	public class CharacterCreatorTogglePoseReference : MonoBehaviour, ICharacterCreatorTogglePoseReference
	{
		public PoseId PoseId { get; set; }

		public string DisplayName => PoseId.DisplayName;

		public Sprite Sprite => PoseId.Preview.Sprite;
	}
}