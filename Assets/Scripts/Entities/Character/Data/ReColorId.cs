using Character.Compositor;
using UnityEngine;

namespace Character.Data
{
	// Pretty much just a stub to have a unique ID
	[CreateAssetMenu(fileName = "", menuName = "Scriptable Objects/Character Data/ReColorId")]
	public class ReColorId : ScriptableObject, IHasUniqueAssetId
	{
		[SerializeField] ColorGroup _colorGroup;
		public ColorGroup ColorGroup => _colorGroup;

		[SerializeField, HideInInspector] string _uniqueAssetId;
		public string UniqueAssetID { get => _uniqueAssetId; set => _uniqueAssetId = value; }
	}
}