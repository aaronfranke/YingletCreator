using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CharacterCompositor
{
	public static class TextureUtilities
	{
		public static void UpdateMaterialsWithTextures(IReadOnlyDictionary<MaterialDescription, Material> materialMapping, IEnumerable<MixTexture> mixTextures)
		{
			var blitShader = Shader.Find("Hidden/Colorize");
			var blitMaterial = new Material(blitShader);

			foreach (var mappedMaterial in materialMapping)
			{
				var materialDescription = mappedMaterial.Key;
				var material = mappedMaterial.Value;

				var applicableMixTextures = mixTextures.Where(m => m.TargetMaterialDescription == materialDescription).ToArray(); // TODO sort this

				int largestSize = applicableMixTextures.Max(m => m.Shading.width);

				// Set up an appropriately sized texture
				var renderTextures = new DoubleBufferedRenderTexture(largestSize);

				foreach (var applicableMixTexture in applicableMixTextures)
				{
					blitMaterial.SetTexture("_MixTex", applicableMixTexture.Shading);
					blitMaterial.SetColorizeParams(applicableMixTexture.DefaultColorGroup.DefaultColorValues);
					renderTextures.Blit(blitMaterial);
				}

				material.mainTexture = renderTextures.Finalize();
			}
		}
	}
}
