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
            var meshMapping = MeshUtilities.GenerateMeshes(this.transform, _rigPrefab, _meshesWithMaterials);
            MaterialUtilities.ApplyMaterialsToMeshes(meshMapping);
        }

        public void Clear()
        {
            MeshUtilities.ClearMeshes(this.transform, _rigPrefab, _meshesWithMaterials);
        }
    }
}
