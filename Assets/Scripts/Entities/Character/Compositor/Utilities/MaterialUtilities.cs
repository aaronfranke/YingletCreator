using System.Collections.Generic;
using UnityEngine;

namespace Character.Compositor
{
	public static class MaterialUtilities
	{
		/// <summary>
		/// Generates and applies the materials needed for the character meshes
		/// </summary>
		/// <returns>A mapping between the material descriptions and their generated materials</returns>
		public static IReadOnlyDictionary<MaterialDescription, Material> ApplyMaterialsToMeshes(IReadOnlyDictionary<MeshWithMaterial, GameObject> meshMapping)
		{
			var materialMap = new Dictionary<MaterialDescription, Material>();
			foreach (var mappedMesh in meshMapping)
			{
				var materialDescription = mappedMesh.Key.MaterialDescription;
				var meshGameObject = mappedMesh.Value;

				if (materialMap.TryGetValue(materialDescription, out Material existingMaterial))
				{
					ApplyMaterial(meshGameObject, existingMaterial);
				}
				else
				{
					var newMaterial = new Material(materialDescription.ReferenceMaterial);
					materialMap[materialDescription] = newMaterial;
					ApplyMaterial(meshGameObject, newMaterial);
				}
			}
			return materialMap;
		}

		static void ApplyMaterial(GameObject meshGameObject, Material material)
		{
			var skinnedMR = meshGameObject.GetComponent<SkinnedMeshRenderer>();
			skinnedMR.sharedMaterial = material;
		}
	}
}
