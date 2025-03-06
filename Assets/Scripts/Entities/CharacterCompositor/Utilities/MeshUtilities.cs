using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CharacterCompositor
{
	public static class MeshUtilities
	{
		/// <summary>
		/// Generates the meshes needed for the character
		/// </summary>
		/// <returns>A mapping between the description of the required meshes and the generated gameobjects</returns>
		public static IReadOnlyDictionary<MeshWithMaterial, GameObject> GenerateMeshes(Transform root, GameObject rigPrefab, IEnumerable<MeshWithMaterial> meshesWithMaterials)
		{
			var rigGo = GameObject.Instantiate(rigPrefab, root);
			rigGo.name = rigPrefab.name;

			var boneMap = GetChildTransformMap(rigGo.transform);

			var mapping = new Dictionary<MeshWithMaterial, GameObject>();
			foreach (var meshWithMaterial in meshesWithMaterials)
			{
				mapping[meshWithMaterial] = CreateSkinnedObject(meshWithMaterial.SkinnedMeshRendererPrefab, root, boneMap);
			}
			return mapping;
		}

		public static void ClearMeshes(Transform root, GameObject rigPrefab, IEnumerable<MeshWithMaterial> meshesWithMaterials)
		{
			root.DeleteChildIfExists(rigPrefab.name);

			foreach (var meshWithMaterial in meshesWithMaterials)
			{
				root.DeleteChildIfExists(meshWithMaterial.SkinnedMeshRendererPrefab.name);
			}
		}


		static void DeleteChildIfExists(this UnityEngine.Transform transform, string name)
		{
			var child = transform.Find(name);
			if (child == null) return;
			GameObject.DestroyImmediate(child.gameObject);
		}

		static Dictionary<string, Transform> GetChildTransformMap(Transform root)
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

		static GameObject CreateSkinnedObject(GameObject prefab, Transform parent, Dictionary<string, Transform> boneMap)
		{
			var bodyGo = GameObject.Instantiate(prefab, parent);
			bodyGo.name = prefab.name;
			var skinnedMeshRenderer = bodyGo.GetComponent<SkinnedMeshRenderer>();
			skinnedMeshRenderer.rootBone = boneMap["root"];
			skinnedMeshRenderer.bones = skinnedMeshRenderer.bones.Select(b => boneMap[b.name]).ToArray();
			return bodyGo;
		}
	}
}
