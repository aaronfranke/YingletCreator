using Character.Compositor;
using Snapshotter;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Character.Data
{
	[CreateAssetMenu(fileName = "Toggle", menuName = "Scriptable Objects/Character Data/CharacterToggleId")]
	public class CharacterToggleId : ScriptableObject, IHasUniqueAssetId, ISnapshottableScriptableObject, IOrderableScriptableObject<CharacterToggleOrderGroup>
	{
		[SerializeField, HideInInspector] string _uniqueAssetId;
		public string UniqueAssetID { get => _uniqueAssetId; set => _uniqueAssetId = value; }

		[SerializeField] string _displayName;
		public string DisplayName => _displayName;

		[SerializeField] AssetReferenceT<CharacterToggleEnforcementGroup>[] _groupReferences;
		public IEnumerable<CharacterToggleEnforcementGroup> Groups => _groupReferences.Select(r => r.LoadSync());

		[SerializeField] AssetReferenceT<MeshWithMaterial>[] _addedMeshReferences;
		public IEnumerable<MeshWithMaterial> AddedMeshes => _addedMeshReferences.Select(r => r.LoadSync());

		[SerializeField] AssetReferenceT<MixTexture>[] _addedTextureReferences;
		public IEnumerable<MixTexture> AddedTextures => _addedTextureReferences.Select(r => r.LoadSync());

		[SerializeField] EyeMixTextures _eyeTextures;
		public EyeMixTextures EyeTextures => _eyeTextures;

		[SerializeField] GameObject _roomPrefab;
		public GameObject RoomPrefab => _roomPrefab;

		[SerializeField] AssetReferenceT<CharacterToggleComponent>[] _componentReferences;
		public IEnumerable<CharacterToggleComponent> Components => _componentReferences.Select(r => r.LoadSync());

		[SerializeField] CharacterTogglePreviewData _preview;
		public CharacterTogglePreviewData Preview => _preview;

		[SerializeField] CharacterToggleOrderData _order;
		public CharacterToggleOrderData Order => _order;
		IOrderData<CharacterToggleOrderGroup> IOrderableScriptableObject<CharacterToggleOrderGroup>.Order => Order;
	}

	[System.Serializable]
	public class CharacterTogglePreviewData
	{
		[SerializeField] Sprite _sprite;
		public Sprite Sprite => _sprite;

		[SerializeField] SnapshotterCameraPosition _cameraPosition;
		public SnapshotterCameraPosition CameraPosition => _cameraPosition;


#if UNITY_EDITOR
		public void SetSprite(Sprite sprite)
		{
			_sprite = sprite;
		}
#endif
	}

	[System.Serializable]
	public class CharacterToggleOrderData : IOrderData<CharacterToggleOrderGroup>
	{
		public CharacterToggleOrderGroup Group => _groupReference.LoadSync();

		[SerializeField] AssetReferenceT<CharacterToggleOrderGroup> _groupReference;

		[SerializeField] int _index;
		public int Index => _index;
	}
}