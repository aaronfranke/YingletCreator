using UnityEngine;

namespace Character.Data
{
	[CreateAssetMenu(fileName = "Toggle", menuName = "Scriptable Objects/Character Data/CharacterToggleId")]
	public class CharacterToggleId : ScriptableObject, IHasUniqueAssetId
	{
		[SerializeField, HideInInspector] string _uniqueAssetId;
		public string UniqueAssetID { get => _uniqueAssetId; set => _uniqueAssetId = value; }

		[SerializeField] string _displayName;
		public string DisplayName => _displayName;
	}
}