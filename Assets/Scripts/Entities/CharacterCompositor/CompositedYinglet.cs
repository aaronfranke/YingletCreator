using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CharacterCompositor
{

    public class CompositedYinglet : MonoBehaviour
    {
        [SerializeField] GameObject _rigPrefab;

        [SerializeField] GameObject _bodyPrefab;

        const string RigChildName = "Rig";
        const string BodyChildName = "Body";

        public void Composite()
        {
            Clear();

            var rigGo = CreateWithName(_rigPrefab, RigChildName);

            // TODO clean this all up
            _bodyPrefab.gameObject.SetActive(false);
            var bodyGo = GameObject.Instantiate(_bodyPrefab, this.transform);
            var skinnedMeshRenderer = bodyGo.GetComponent<SkinnedMeshRenderer>();
            skinnedMeshRenderer.rootBone = rigGo.transform.Find("root");
            var boneMap = GetChildTransformMap(rigGo.transform);
            skinnedMeshRenderer.bones = skinnedMeshRenderer.bones.Select(b => boneMap[b.name]).ToArray();
            bodyGo.name = BodyChildName;
            bodyGo.gameObject.SetActive(true);
            _bodyPrefab.gameObject.SetActive(true);

        }

        public void Clear()
        {
            DeleteChildIfExists(RigChildName);
            DeleteChildIfExists(BodyChildName);
        }

        GameObject CreateWithName(GameObject prefab, string name)
        {
            var obj = GameObject.Instantiate(prefab, this.transform);
            obj.name = name;

            return obj;
        }

        void DeleteChildIfExists(string name)
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
    }
}
