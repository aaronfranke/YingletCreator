using Reactivity;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Character.Compositor
{
	public interface IMeshGeneration
	{
		IEnumerable<MeshObjectWithMaterialDescription> Meshes { get; }

	}
	public class MeshGeneration : ReactiveBehaviour, IMeshGeneration
	{
		[SerializeField] Transform _rigRoot;
		private IMeshGatherer _meshDefinition;
		private EnumerableReflector<MeshWithMaterial, MeshObjectWithMaterialDescription> _enumerableReflector;
		private Dictionary<string, Transform> _boneMap;

		IEnumerable<MeshObjectWithMaterialDescription> IMeshGeneration.Meshes => _enumerableReflector.Values;

		void Awake()
		{
			_meshDefinition = this.GetComponent<IMeshGatherer>();
			_enumerableReflector = new EnumerableReflector<MeshWithMaterial, MeshObjectWithMaterialDescription>(Added, Removed);
			_boneMap = GetBoneMap(_rigRoot);
			AddReflector(Composite);
		}
		private MeshObjectWithMaterialDescription Added(MeshWithMaterial mesh)
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
			return new MeshObjectWithMaterialDescription(newGO, mesh.MaterialDescription);
		}
		private void Removed(MeshObjectWithMaterialDescription obj)
		{
			Destroy(obj.MeshGO);
		}

		public void Composite()
		{
			var meshes = _meshDefinition.AllRelevantMeshes.ToArray();
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

	public sealed class MeshObjectWithMaterialDescription
	{
		public MeshObjectWithMaterialDescription(GameObject mesh, MaterialDescription materialDescription)
		{
			MeshGO = mesh;
			MaterialDescription = materialDescription;
		}

		public GameObject MeshGO { get; }
		public MaterialDescription MaterialDescription { get; }
	}
}
