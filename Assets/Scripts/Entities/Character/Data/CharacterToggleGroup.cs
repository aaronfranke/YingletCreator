using UnityEngine;

namespace Character.Data
{
	[CreateAssetMenu(fileName = "Group", menuName = "Scriptable Objects/Character Data/CharacterToggleGroup")]
	public class CharacterToggleGroup : ScriptableObject
	{
		[SerializeField] bool _mustHaveOne;
		public bool MustHaveOne => _mustHaveOne;
	}
}