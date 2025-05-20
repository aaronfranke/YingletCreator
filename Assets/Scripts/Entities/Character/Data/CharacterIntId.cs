
using UnityEngine;

namespace Character.Data
{
	// Pretty much just a stub to have a unique ID
	[CreateAssetMenu(fileName = "Int", menuName = "Scriptable Objects/Character Data/CharacterIntId")]
	public class CharacterIntId : ScriptableObject, IHasUniqueAssetId
	{
		[SerializeField, HideInInspector] string _uniqueAssetId;
		public string UniqueAssetID { get => _uniqueAssetId; set => _uniqueAssetId = value; }
	}
}