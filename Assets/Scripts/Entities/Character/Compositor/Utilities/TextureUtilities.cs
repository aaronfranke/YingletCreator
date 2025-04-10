using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CharacterCompositor
{
	public static class TextureUtilities
	{
		static readonly int MASK_TEX_PROPERTY_ID = Shader.PropertyToID("_MaskTex");
		static readonly int GRAYSCALE_TEX_PROPERTY_ID = Shader.PropertyToID("_GrayscaleTex");


		static readonly int MAIN_COLOR_PROPERTY_ID = Shader.PropertyToID("_Color");
		static readonly int DARK_COLOR_PROPERTY_ID = Shader.PropertyToID("_DarkColor");

		static readonly int OUTLINE_PROPERTY_ID = Shader.PropertyToID("_Outline");
		static readonly int PUPIL_PROPERTY_ID = Shader.PropertyToID("_Pupil");

		public static void UpdateMaterialsWithTextures(IReadOnlyDictionary<MaterialDescription, Material> materialMapping, IEnumerable<IMixTexture> mixTextures, MixTextureOrdering mixTextureOrdering)
		{
			var colorizeShader = Shader.Find("CharacterCompositor/Colorize");
			var colorizeWithMaskShader = Shader.Find("CharacterCompositor/ColorizeWithMask");
			var blitMaterial = new Material(colorizeShader);

			var sortedMixTextures = SortMixTextures(mixTextures, mixTextureOrdering);

			foreach (var mappedMaterial in materialMapping)
			{
				UpdateMaterial(mappedMaterial.Key, mappedMaterial.Value);
			}

			void UpdateMaterial(MaterialDescription materialDescription, Material material)
			{
				var allApplicableMixTextures = sortedMixTextures.Where(m => m.TargetMaterialDescription == materialDescription).ToArray();
				if (!allApplicableMixTextures.Any())
				{
					Debug.LogWarning($"Failed to texture material '{materialDescription.name}'; no applicable mix textures to apply");
				}

				// Some materials have multiple textures (notably the eyes). Update them separately
				var buckets = allApplicableMixTextures.GroupBy(p => p.TargetMaterialTexture);
				foreach (var bucket in buckets)
				{
					if (!bucket.Any()) continue;
					UpdateMaterialTexture(bucket.Key, bucket);
				}


				void UpdateMaterialTexture(TargetMaterialTexture materialTexture, IEnumerable<IMixTexture> applicableMixTextures)
				{
					int largestSize = applicableMixTextures.Any() ? applicableMixTextures.Max(m => m.Grayscale.width) : 1;
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
							blitMaterial.SetTexture(MASK_TEX_PROPERTY_ID, applicableMixTexture.Mask);
						}
						blitMaterial.SetTexture(GRAYSCALE_TEX_PROPERTY_ID, applicableMixTexture.Grayscale);
						ApplyMixTexturePropsToMaterial(blitMaterial, applicableMixTexture);
						renderTextures.Blit(blitMaterial);
					}

					if (materialTexture == TargetMaterialTexture.MainTexture)
						material.mainTexture = renderTextures.Finalize();
					else if (materialTexture == TargetMaterialTexture.Outline)
						material.SetTexture(OUTLINE_PROPERTY_ID, renderTextures.Finalize());
					else if (materialTexture == TargetMaterialTexture.Pupil)
						material.SetTexture(PUPIL_PROPERTY_ID, renderTextures.Finalize());
				}
			}
		}

		static void ApplyMixTexturePropsToMaterial(Material material, IMixTexture mixTexture)
		{
			var values = mixTexture.DefaultColorGroup.DefaultColorValues;
			material.SetColor(MAIN_COLOR_PROPERTY_ID, Color.red);
			material.SetColor(DARK_COLOR_PROPERTY_ID, Color.blue);
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
