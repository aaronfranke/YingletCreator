using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CharacterCompositor
{

    public class CompositedYinglet : MonoBehaviour
    {
        [SerializeField] GameObject _rigPrefab;

        [SerializeField] MeshWithMaterial[] _meshesWithMaterials;

        public void Composite()
        {
            Clear();

            var rigGo = GameObject.Instantiate(_rigPrefab, this.transform);
            rigGo.name = _rigPrefab.name;

            var boneMap = Utilities.GetChildTransformMap(rigGo.transform);

            foreach (var meshWithMaterial in _meshesWithMaterials)
            {
                Utilities.CreateSkinnedObject(meshWithMaterial.SkinnedMeshRendererPrefab, this.transform, boneMap);
            }
        }

        public void Clear()
        {
            this.transform.DeleteChildIfExists(_rigPrefab.name);
            
            foreach (var meshWithMaterial in _meshesWithMaterials)
            {
                this.transform.DeleteChildIfExists(meshWithMaterial.SkinnedMeshRendererPrefab.name);
            }
        }

    }
}
