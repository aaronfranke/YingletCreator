using UnityEngine;

namespace CharacterCompositor
{

    public class CompositedYinglet : MonoBehaviour
    {
        [SerializeField] GameObject _rigPrefab;
        [SerializeField] MeshWithMaterial[] _meshesWithMaterials;
        [SerializeField] MixTexture[] _mixTextures;

        public void Composite()
        {
            Clear();
            var meshMapping = MeshUtilities.GenerateMeshes(this.transform, _rigPrefab, _meshesWithMaterials);
            var materialMapping = MaterialUtilities.ApplyMaterialsToMeshes(meshMapping);
            TextureUtilities.UpdateMaterialsWithTextures(materialMapping, _mixTextures);
        }

        public void Clear()
        {
            MeshUtilities.ClearMeshes(this.transform, _rigPrefab, _meshesWithMaterials);
        }
    }
}
