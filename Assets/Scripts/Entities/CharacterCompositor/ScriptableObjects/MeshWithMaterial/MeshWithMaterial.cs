using UnityEngine;

namespace CharacterCompositor
{
	public interface IMeshWithMaterial
	{
		GameObject SkinnedMeshRendererPrefab { get; }
		MaterialDescription MaterialDescription { get; }
	}

	[CreateAssetMenu(fileName = "MeshWithMaterial", menuName = "Scriptable Objects/Character Compositor/MeshWithMaterial")]
	public class MeshWithMaterial : ScriptableObject, IMeshWithMaterial
	{
		[SerializeField] GameObject _skinnedMeshRendererPrefab;
		[SerializeField] MaterialDescription _materialWithDescription;

		public GameObject SkinnedMeshRendererPrefab => _skinnedMeshRendererPrefab;

        public MaterialDescription MaterialDescription => _materialWithDescription;
    }
}
