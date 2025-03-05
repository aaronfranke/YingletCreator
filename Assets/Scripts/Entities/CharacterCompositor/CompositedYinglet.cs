using UnityEngine;

namespace CharacterCompositor
{

    public class CompositedYinglet : MonoBehaviour
    {
        [SerializeField] GameObject _rigPrefab;

        const string RigChildName = "Rig";

        public void Composite()
        {
            Clear();

            var obj = GameObject.Instantiate(_rigPrefab, this.transform);
            obj.name = RigChildName;
        }

        public void Clear()
        {
            DeleteChildIfExists(RigChildName);
        }

        void DeleteChildIfExists(string name)
        {
            var child = transform.Find(name);
            if (child == null) return;
            GameObject.DestroyImmediate(child.gameObject);
        }
    }
}
