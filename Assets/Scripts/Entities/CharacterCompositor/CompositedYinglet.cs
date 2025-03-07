using UnityEngine;

namespace CharacterCompositor
{

    public class CompositedYinglet : MonoBehaviour
    {
        [SerializeField] GameObject _rigPrefab;
        [SerializeField] MeshWithMaterial[] _meshesWithMaterials;
        [SerializeField] MixTexture[] _mixTextures;
        [SerializeField] MixTextureOrdering _mixTextureOrdering;

        public void Composite()
        {
            Clear();
            var meshMapping = MeshUtilities.GenerateMeshes(this.transform, _rigPrefab, _meshesWithMaterials);
            var materialMapping = MaterialUtilities.ApplyMaterialsToMeshes(meshMapping);
            TextureUtilities.UpdateMaterialsWithTextures(materialMapping, _mixTextures, _mixTextureOrdering);
        }

        public void Clear()
        {
            MeshUtilities.ClearMeshes(this.transform, _rigPrefab, _meshesWithMaterials);
        }
    }
}
