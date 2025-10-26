using Character.Compositor;
using Snapshotter;
using System.Collections.Generic;
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

		[SerializeField] public CharacterToggleEnforcementGroup[] _groups;
		public CharacterToggleEnforcementGroup[] Groups => _groups;

		[SerializeField] MeshWithMaterial[] _addedMeshes;
		public IEnumerable<MeshWithMaterial> AddedMeshes => _addedMeshes;

		[SerializeField] MixTexture[] _addedTextures;
		public IEnumerable<MixTexture> AddedTextures => _addedTextures;

		[SerializeField] EyeMixTextures _eyeTextures;
		public EyeMixTextures EyeTextures => _eyeTextures;

		[SerializeField] GameObject _roomPrefab;
		public GameObject RoomPrefab => _roomPrefab;

		[SerializeField] CharacterToggleComponent[] _components;
		public CharacterToggleComponent[] Components => _components;

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