using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace Character.Compositor
{

	[CreateAssetMenu(fileName = "MeshWithMaterial", menuName = "Scriptable Objects/Character Compositor/MeshWithMaterial")]
	public class MeshWithMaterial : ScriptableObject, ITaggableCharacterElement
	{
		[SerializeField] GameObject _skinnedMeshRendererPrefab;
		[SerializeField] AssetReferenceT<MaterialDescription> _materialWithDescriptionReference;
		[SerializeField] AssetReferenceT<CharacterElementTag>[] _tagReferences;
		[SerializeField] AssetReferenceT<CharacterBone> _boneToAttachToReference;

		public GameObject SkinnedMeshRendererPrefab => _skinnedMeshRendererPrefab;
		public MaterialDescription MaterialDescription => _materialWithDescriptionReference.LoadSync();
		public IEnumerable<CharacterElementTag> Tags => _tagReferences.Select(r => r.LoadSync());
		public CharacterBone BoneToAttachTo => _boneToAttachToReference.LoadSync();

#if UNITY_EDITOR
		public void EditorSetSkinnedMeshRendererPrefab(GameObject prefab)
		{
			_skinnedMeshRendererPrefab = prefab;
		}
#endif
	}
}
