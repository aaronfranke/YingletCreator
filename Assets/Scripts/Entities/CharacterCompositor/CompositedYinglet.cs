using System.Collections.Generic;
using UnityEngine;

namespace CharacterCompositor
{

    public class CompositedYinglet : MonoBehaviour
    {
        [SerializeField] GameObject _rigPrefab;
        [SerializeField] MeshWithMaterial[] _meshesWithMaterials;
        [SerializeField] MixTexture[] _mixTextures;
        [SerializeField] MixTextureOrdering _mixTextureOrdering;

        IReadOnlyDictionary<MeshWithMaterial, GameObject> _lastMeshMapping;
        IReadOnlyDictionary<MaterialDescription, Material> _lastMaterialMapping;

        public void Composite()
        {
            Clear();
            _lastMeshMapping = MeshUtilities.GenerateMeshes(this.transform, _rigPrefab, _meshesWithMaterials);
            _lastMaterialMapping = MaterialUtilities.ApplyMaterialsToMeshes(_lastMeshMapping);
            TextureUtilities.UpdateMaterialsWithTextures(_lastMaterialMapping, _mixTextures, _mixTextureOrdering);
        }

        public void Clear()
        {
            MeshUtilities.ClearMeshes(this.transform, _rigPrefab, _meshesWithMaterials);
        }

        public void UpdateColorGroup()
        {
            // Optimization opportunity: Pass in the color group and use it to filter materials that need updating
            TextureUtilities.UpdateMaterialsWithTextures(_lastMaterialMapping, _mixTextures, _mixTextureOrdering);
        }
    }
}
