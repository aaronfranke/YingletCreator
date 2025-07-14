using UnityEngine;

namespace Character.Data
{
	/// <summary>
	/// This group, if defined, conrols what toggles should also be applyable
	/// i.e. if two elements are in the same group, they can remove each other when selected
	/// </summary>
	[CreateAssetMenu(fileName = "Group", menuName = "Scriptable Objects/Character Data/CharacterToggleEnforcementGroup")]
	public class CharacterToggleEnforcementGroup : ScriptableObject
	{
		[SerializeField] bool _mustHaveOne;
		public bool MustHaveOne => _mustHaveOne;
	}
}