using Reactivity;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CharacterCompositor
{
	public class MeshGeneration : ReactiveBehaviour
	{
		[SerializeField] Transform _rigRoot;
		private IMeshDefinition _meshDefinition;
		private EnumerableReflector<MeshWithMaterial, GameObject> _enumerableReflector;
		private Dictionary<string, Transform> _boneMap;

		void Awake()
		{
			_meshDefinition = this.GetComponent<IMeshDefinition>();
			_enumerableReflector = new EnumerableReflector<MeshWithMaterial, GameObject>(Added, Removed);
			_boneMap = GetBoneMap(_rigRoot);
			AddReflector(Composite);
		}
		private GameObject Added(MeshWithMaterial mesh)
		{

			var newGO = GameObject.Instantiate(mesh.SkinnedMeshRendererPrefab, this.transform.position, Quaternion.identity, this.transform);


			newGO.name = mesh.SkinnedMeshRendererPrefab.name;
			var skinnedMeshRenderer = newGO.GetComponent<SkinnedMeshRenderer>();
			skinnedMeshRenderer.rootBone = _boneMap["root"];
			skinnedMeshRenderer.bones = skinnedMeshRenderer.bones.Select(b =>
			{
				return _boneMap[b.name];
			}
			).ToArray();
			//mapping[meshWithMaterial] = CreateSkinnedObject(meshWithMaterial.SkinnedMeshRendererPrefab, meshRoot.transform, boneMap);
			return newGO;
		}
		private void Removed(GameObject obj)
		{
			GameObject.Destroy(obj);
		}

		public void Composite()
		{
			var meshes = _meshDefinition.DefinedMeshes.ToArray();
			_enumerableReflector.Enumerate(meshes);
		}


		public static Dictionary<string, Transform> GetBoneMap(Transform root)
		{
			var transformMap = new Dictionary<string, Transform>();
			AddChildrenToMap(root);
			return transformMap;

			void AddChildrenToMap(Transform parent)
			{
				foreach (Transform child in parent)
				{
					transformMap[child.name] = child;
					AddChildrenToMap(child);
				}
			}
		}
	}
}
