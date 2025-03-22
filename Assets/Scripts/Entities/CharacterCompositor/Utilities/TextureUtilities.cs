using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CharacterCompositor
{
	public static class TextureUtilities
	{
		static readonly int HUE_SHIFT_PROPERTY_ID = Shader.PropertyToID("_HueShift");
		static readonly int MULTIPLICATION_PROPERTY_ID = Shader.PropertyToID("_Multiplication");
		static readonly int CONTRAST_PROPERTY_ID = Shader.PropertyToID("_Contrast");
		static readonly int SATURATION_PROPERTY_ID = Shader.PropertyToID("_Saturation");
		static readonly int CONTRAST_MIDPOINT_PROPERTY_ID = Shader.PropertyToID("_ContrastMidpoint");

		public static void UpdateMaterialsWithTextures(IReadOnlyDictionary<MaterialDescription, Material> materialMapping, IEnumerable<IMixTexture> mixTextures, MixTextureOrdering mixTextureOrdering)
		{
			var colorizeShader = Shader.Find("CharacterCompositor/Colorize");
			var colorizeWithMaskShader = Shader.Find("CharacterCompositor/ColorizeWithMask");
			var blitMaterial = new Material(colorizeShader);

			var sortedMixTextures = SortMixTextures(mixTextures, mixTextureOrdering);

			foreach (var mappedMaterial in materialMapping)
			{
				var materialDescription = mappedMaterial.Key;
				var material = mappedMaterial.Value;

				var applicableMixTextures = sortedMixTextures.Where(m => m.TargetMaterialDescription == materialDescription).ToArray();
				if (!applicableMixTextures.Any())
				{
					Debug.LogWarning($"Failed to texture material '{materialDescription.name}'; no applicable mix textures to apply");
				}

				int largestSize = applicableMixTextures.Any() ? applicableMixTextures.Max(m => m.Shading.width) : 1;
				var renderTextures = new DoubleBufferedRenderTexture(largestSize);

				foreach (var applicableMixTexture in applicableMixTextures)
				{
					if (applicableMixTexture.Mask == null)
					{
						blitMaterial.shader = colorizeShader;
					}
					else
					{
						blitMaterial.shader = colorizeWithMaskShader;
						blitMaterial.SetTexture("_MaskTex", applicableMixTexture.Mask);
					}
					blitMaterial.SetTexture("_MixTex", applicableMixTexture.Shading);
					ApplyMixTexturePropsToMaterial(blitMaterial, applicableMixTexture);
					renderTextures.Blit(blitMaterial);
				}

				material.mainTexture = renderTextures.Finalize();
			}
		}

		static void ApplyMixTexturePropsToMaterial(Material material, IMixTexture mixTexture)
		{
			var values = mixTexture.DefaultColorGroup.DefaultColorValues;
			material.SetFloat(HUE_SHIFT_PROPERTY_ID, values.HueShift);
			material.SetFloat(MULTIPLICATION_PROPERTY_ID, values.Multiplication);
			material.SetFloat(CONTRAST_PROPERTY_ID, values.Contrast);
			material.SetFloat(SATURATION_PROPERTY_ID, values.Saturation);
			material.SetColor(CONTRAST_MIDPOINT_PROPERTY_ID, mixTexture.ContrastMidpointColor);
		}

		static IEnumerable<IMixTexture> SortMixTextures(IEnumerable<IMixTexture> mixTextures, MixTextureOrdering mixTextureOrdering)
		{
			var mixTextureToOrder = new Dictionary<IMixTexture, int>();
			for (int i = 0; i < mixTextureOrdering.OrderedMixTextures.Length; i++)
			{
				mixTextureToOrder[mixTextureOrdering.OrderedMixTextures[i]] = i;
			}
			return mixTextures.OrderBy(m =>
			{
				if (!m.Sortable) return 0;
				
				if (mixTextureToOrder.TryGetValue(m, out int index))
				{
					return index;
				}
				Debug.LogWarning($"MixTexture '{m.name}' did not exist in the mix texture ordering. Add it to the `_Order` scriptable object");
				return int.MaxValue;
			}).ToArray();
		}
	}
}
