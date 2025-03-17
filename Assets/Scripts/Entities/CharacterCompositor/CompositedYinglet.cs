using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CharacterCompositor
{

    public class CompositedYinglet : MonoBehaviour
    {
        [SerializeField] GameObject _rigPrefab;
        [SerializeField] MeshWithMaterial[] _meshesWithMaterials;
        [SerializeField] MixTexture[] _mixTextures;
        [SerializeField] EyeMixTextures _eyeMixTexture;
        [SerializeField] EyeMixTextureReferences _eyeMixTextureReferences;
        [SerializeField] MixTextureOrdering _mixTextureOrdering;

        IReadOnlyDictionary<MeshWithMaterial, GameObject> _lastMeshMapping;
        IReadOnlyDictionary<MaterialDescription, Material> _lastMaterialMapping;

        void Start()
        {
            Composite();
        }

        public void Composite()
        {
            Clear();
            _lastMeshMapping = MeshUtilities.GenerateMeshes(this.transform, _rigPrefab, _meshesWithMaterials);
            _lastMaterialMapping = MaterialUtilities.ApplyMaterialsToMeshes(_lastMeshMapping);
            UpdateColorGroup();
            _eyeMixTexture.ApplyEyeProperties(_lastMaterialMapping, _eyeMixTextureReferences);
        }

        public void Clear()
        {
            MeshUtilities.ClearMeshes(this.transform, _rigPrefab);
        }

        public void UpdateColorGroup()
        {
            // Optimization opportunity: Pass in the color group and use it to filter materials that need updating
            IEnumerable<IMixTexture> mixTextures = _mixTextures.Concat(_eyeMixTexture.GenerateMixTextures(_eyeMixTextureReferences)).ToArray();
            TextureUtilities.UpdateMaterialsWithTextures(_lastMaterialMapping, mixTextures, _mixTextureOrdering);
        }
    }
}
