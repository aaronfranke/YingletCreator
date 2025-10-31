using Character.Compositor;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Character.Data
{
	// Pretty much just a stub to have a unique ID
	[CreateAssetMenu(fileName = "", menuName = "Scriptable Objects/Character Data/ReColorId")]
	public class ReColorId : ScriptableObject, IHasUniqueAssetId
	{
		[SerializeField] AssetReferenceT<ColorGroup> _colorGroupReference;
		public ColorGroup ColorGroup => _colorGroupReference.LoadSync();

		[SerializeField, HideInInspector] string _uniqueAssetId;
		public string UniqueAssetID { get => _uniqueAssetId; set => _uniqueAssetId = value; }

		[SerializeField] string _overrideName;
		public string DisplayName => string.IsNullOrWhiteSpace(_overrideName) ? this.name : _overrideName;

		[SerializeField] bool _cleanupIfUnused = true;
		public bool CleanupIfUnused => _cleanupIfUnused;
	}
}