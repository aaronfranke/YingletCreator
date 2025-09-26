using Reactivity;
using UnityEngine;

namespace Character.Compositor
{
    public class MaterialApplier : ReactiveBehaviour
    {
        private IMeshGeneration _meshGeneration;
        private IMaterialGeneration _materialGeneration;

        private void Awake()
        {
            _meshGeneration = this.GetCompositedYingletComponent<IMeshGeneration>();
            _materialGeneration = this.GetComponent<IMaterialGeneration>();
            AddReflector(Reflect);
        }

        private void Reflect()
        {
            // Could probably be optimized to not re-apply to everything but w/e
            var lookup = _materialGeneration.GeneratedMaterialLookup;
            var meshes = _meshGeneration.Meshes;
            foreach (var mesh in meshes)
            {
                if (mesh.MaterialDescription == null)
                {
                    // Props might not have a material description, don't try to replace them
                    continue;
                }
                if (lookup.TryGetValue(mesh.MaterialDescription, out Material material))
                {
                    mesh.MeshGO.GetComponent<Renderer>().material = material;
                }
            }
        }
    }

}