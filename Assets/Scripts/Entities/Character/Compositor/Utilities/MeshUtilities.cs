using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace CharacterCompositor
{
    public static class MeshUtilities
    {
        const string MESH_ROOT_NAME = "Meshes";

        /// <summary>
        /// Generates the meshes needed for the character
        /// </summary>
        /// <returns>A mapping between the description of the required meshes and the generated gameobjects</returns>
        public static IReadOnlyDictionary<MeshWithMaterial, GameObject> GenerateMeshes(Transform root, Transform rigRoot, IEnumerable<MeshWithMaterial> meshesWithMaterials)
        {
            var meshRoot = new GameObject(MESH_ROOT_NAME);
            meshRoot.transform.parent = root;
            meshRoot.transform.localPosition = Vector3.zero;

            var boneMap = GetChildTransformMap(rigRoot);

            var mapping = new Dictionary<MeshWithMaterial, GameObject>();
            foreach (var meshWithMaterial in meshesWithMaterials)
            {
                mapping[meshWithMaterial] = CreateSkinnedObject(meshWithMaterial.SkinnedMeshRendererPrefab, meshRoot.transform, boneMap);
            }
            return mapping;
        }

        public static void ClearMeshes(Transform root)
        {
            root.DeleteChildIfExists(MESH_ROOT_NAME);
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
            skinnedMeshRenderer.bones = skinnedMeshRenderer.bones.Select(b =>
            {
                return boneMap[b.name];
            }
            ).ToArray();
            return bodyGo;
        }
    }
}
