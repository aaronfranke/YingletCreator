using Character.Data;
using System.Collections.Generic;
using UnityEngine;

namespace Character.Compositor
{
	public interface ICharacterElementTagMask
	{
		IEnumerable<CharacterElementTag> MaskedTags { get; }
	}

	[CreateAssetMenu(fileName = "CharacterElementTagMask", menuName = "Scriptable Objects/Character Compositor/CharacterElementTagMask")]
	public class CharacterElementTagMask : CharacterToggleComponent, ICharacterElementTagMask
	{
		[SerializeField] CharacterElementTag[] _maskedTags;  // TTODO
		public IEnumerable<CharacterElementTag> MaskedTags => _maskedTags;
	}

}
