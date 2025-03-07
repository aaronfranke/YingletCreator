using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CharacterCompositor
{
	public static class TextureUtilities
	{
		public static void UpdateMaterialsWithTextures(IReadOnlyDictionary<MaterialDescription, Material> materialMapping, IEnumerable<MixTexture> mixTextures, MixTextureOrdering mixTextureOrdering)
		{
			var blitShader = Shader.Find("Hidden/Colorize");
			var blitMaterial = new Material(blitShader);

			var sortedMixTextures = SortMixTextures(mixTextures, mixTextureOrdering);

			foreach (var mappedMaterial in materialMapping)
			{
				var materialDescription = mappedMaterial.Key;
				var material = mappedMaterial.Value;

				var applicableMixTextures = sortedMixTextures.Where(m => m.TargetMaterialDescription == materialDescription).ToArray();

				int largestSize = applicableMixTextures.Max(m => m.Shading.width);
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

		static IEnumerable<MixTexture> SortMixTextures(IEnumerable<MixTexture> mixTextures, MixTextureOrdering mixTextureOrdering)
		{
			var mixTextureToOrder = new Dictionary<MixTexture, int>();
			for (int i = 0; i < mixTextureOrdering.OrderedMixTextures.Length; i++)
			{
				mixTextureToOrder[mixTextureOrdering.OrderedMixTextures[i]] = 0;
			}
			return mixTextures.OrderBy(m =>
			{
				if (mixTextureToOrder.TryGetValue(m, out int index))
				{
					return index;
				}
				Debug.LogWarning($"MixTexture '{m.name}' did not exist in the mix texture ordering. Add it to the `_Order` scriptable object", m);
				return int.MaxValue;
			}).ToArray();
		}
	}
}
