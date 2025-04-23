using System.Collections.Generic;
using UnityEngine;

namespace Character.Compositor
{

	[CreateAssetMenu(fileName = "MeshWithMaterial", menuName = "Scriptable Objects/Character Compositor/MeshWithMaterial")]
	public class MeshWithMaterial : ScriptableObject
	{
		[SerializeField] GameObject _skinnedMeshRendererPrefab;
		[SerializeField] MaterialDescription _materialWithDescription;
		[SerializeField] MeshTag[] _tags;
		[SerializeField] MeshTag[] _maskedTags;

		public GameObject SkinnedMeshRendererPrefab => _skinnedMeshRendererPrefab;
		public MaterialDescription MaterialDescription => _materialWithDescription;
		public IEnumerable<MeshTag> Tags => _tags;
		public IEnumerable<MeshTag> MaskedTags => _maskedTags;
	}
}
