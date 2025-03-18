using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CharacterCompositor
{
    public interface ICompositedYinglet
    {
        event Action OnSkinnedMeshRenderersRegenerated;
    }
    public class CompositedYinglet : MonoBehaviour, ICompositedYinglet
    {
        [SerializeField] Transform _rigRoot;
        [SerializeField] MeshWithMaterial[] _meshesWithMaterials;
        [SerializeField] MixTexture[] _mixTextures;
        [SerializeField] EyeMixTextures _eyeMixTexture;
        [SerializeField] EyeMixTextureReferences _eyeMixTextureReferences;
        [SerializeField] MixTextureOrdering _mixTextureOrdering;

        IReadOnlyDictionary<MeshWithMaterial, GameObject> _lastMeshMapping;
        IReadOnlyDictionary<MaterialDescription, Material> _lastMaterialMapping;

        public event Action OnSkinnedMeshRenderersRegenerated = delegate { };

        void Start()
        {
            Composite();
        }

        public void Composite()
        {
            Clear();
            _lastMeshMapping = MeshUtilities.GenerateMeshes(this.transform, _rigRoot, _meshesWithMaterials);
            OnSkinnedMeshRenderersRegenerated();
            _lastMaterialMapping = MaterialUtilities.ApplyMaterialsToMeshes(_lastMeshMapping);
            UpdateColorGroup();
            _eyeMixTexture.ApplyEyeProperties(_lastMaterialMapping, _eyeMixTextureReferences);
        }

        public void Clear()
        {
            MeshUtilities.ClearMeshes(this.transform);
        }

        public void UpdateColorGroup()
        {
            // Optimization opportunity: Pass in the color group and use it to filter materials that need updating
            IEnumerable<IMixTexture> mixTextures = _mixTextures.Concat(_eyeMixTexture.GenerateMixTextures(_eyeMixTextureReferences)).ToArray();
            TextureUtilities.UpdateMaterialsWithTextures(_lastMaterialMapping, mixTextures, _mixTextureOrdering);
        }
    }
}
