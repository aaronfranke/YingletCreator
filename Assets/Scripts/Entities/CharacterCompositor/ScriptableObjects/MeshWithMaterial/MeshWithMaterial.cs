using UnityEngine;

namespace CharacterCompositor
{
	public interface IMeshWithMaterial
	{
		GameObject SkinnedMeshRendererPrefab { get; }
	}

	[CreateAssetMenu(fileName = "MeshWithMaterial", menuName = "Scriptable Objects/Character Compositor/MeshWithMaterial")]
	public class MeshWithMaterial : ScriptableObject, IMeshWithMaterial
	{
		[SerializeField] GameObject _skinnedMeshRendererPrefab;

		public GameObject SkinnedMeshRendererPrefab => _skinnedMeshRendererPrefab;
    }
}
