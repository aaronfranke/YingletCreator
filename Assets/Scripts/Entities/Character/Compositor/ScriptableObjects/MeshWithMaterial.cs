using UnityEngine;

namespace Character.Compositor
{

	[CreateAssetMenu(fileName = "MeshWithMaterial", menuName = "Scriptable Objects/Character Compositor/MeshWithMaterial")]
	public class MeshWithMaterial : ScriptableObject
	{
		[SerializeField] GameObject _skinnedMeshRendererPrefab;
		[SerializeField] MaterialDescription _materialWithDescription;

		public GameObject SkinnedMeshRendererPrefab => _skinnedMeshRendererPrefab;

        public MaterialDescription MaterialDescription => _materialWithDescription;
    }
}
