using System.Collections.Generic;
using UnityEngine;

namespace Character.Compositor
{
	[CreateAssetMenu(fileName = "CharacterElementTag", menuName = "Scriptable Objects/Character Compositor/CharacterElementTag")]
	public class CharacterElementTag : ScriptableObject
	{

	}

	public interface ITaggableCharacterElement
	{
		IEnumerable<CharacterElementTag> Tags { get; }
	}

}
