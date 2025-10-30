using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Character.Compositor
{

	[CreateAssetMenu(fileName = "MeshWithMaterial", menuName = "Scriptable Objects/Character Compositor/MeshWithMaterial")]
	public class MeshWithMaterial : ScriptableObject, ITaggableCharacterElement
	{
		[SerializeField] GameObject _skinnedMeshRendererPrefab;
		[SerializeField] AssetReferenceT<MaterialDescription> _materialWithDescriptionReference;
		[SerializeField] CharacterElementTag[] _tags;  // TTODO
		[SerializeField] AssetReferenceT<CharacterBone> _boneToAttachToReference;

		public GameObject SkinnedMeshRendererPrefab => _skinnedMeshRendererPrefab;
		public MaterialDescription MaterialDescription => _materialWithDescriptionReference.LoadSync();
		public IEnumerable<CharacterElementTag> Tags => _tags;
		public CharacterBone BoneToAttachTo => _boneToAttachToReference.LoadSync();
	}
}
