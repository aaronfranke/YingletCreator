using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CharacterCompositor
{
	public static class Utilities
	{
		public static void DeleteChildIfExists(this Transform transform, string name)
		{
			var child = transform.Find(name);
			if (child == null) return;
			GameObject.DestroyImmediate(child.gameObject);
		}


		public static Dictionary<string, Transform> GetChildTransformMap(Transform root)
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

		
        public static GameObject CreateSkinnedObject(GameObject prefab, Transform parent, Dictionary<string, Transform> boneMap)
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
