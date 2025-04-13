
using Reactivity;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Character.Compositor
{
	internal sealed class IndividualMaterialTexturer : IDisposable
	{
		public static readonly Shader COLORIZE_SHADER = Shader.Find("CharacterCompositor/Colorize");
		public static readonly Shader COLORIZE_WITH_MASK_SHADER = Shader.Find("CharacterCompositor/ColorizeWithMask");

		static readonly int MASK_TEX_PROPERTY_ID = Shader.PropertyToID("_MaskTex");
		static readonly int GRAYSCALE_TEX_PROPERTY_ID = Shader.PropertyToID("_GrayscaleTex");

		static readonly int MAIN_COLOR_PROPERTY_ID = Shader.PropertyToID("_Color");
		static readonly int DARK_COLOR_PROPERTY_ID = Shader.PropertyToID("_DarkColor");

		static readonly int OUTLINE_PROPERTY_ID = Shader.PropertyToID("_Outline");
		static readonly int PUPIL_PROPERTY_ID = Shader.PropertyToID("_Pupil");

		private IndividualMaterialTexturerReferences _references;
		private MaterialWithDescription _material;
		EnumerableSetReflector<IMixTexture> _relevantMixTextures;
		private Reflector _reflectRelevantMixTextures;
		private Reflector _reflectTextureComposite;
		private List<RenderTexture> _cachedRenderTextures = new List<RenderTexture>();

		public IndividualMaterialTexturer(IndividualMaterialTexturerReferences references, MaterialWithDescription material)
		{
			_references = references;
			_material = material;

			_relevantMixTextures = new EnumerableSetReflector<IMixTexture>();
			_reflectRelevantMixTextures = new Reflector(ReflectRelevantMixTextures);
			_reflectTextureComposite = new Reflector(ReflectTextureComposite);
		}

		private void ReflectRelevantMixTextures()
		{
			var allTextures = _references.TextureGatherer.AllRelevantTextures.ToArray();
			var relevantTextures = allTextures.Where(t => t.TargetMaterialDescription == _material.Description).ToArray();
			_relevantMixTextures.Enumerate(relevantTextures);
		}

		private void ReflectTextureComposite()
		{
			CleanupRenderTextures();

			var relevantTextures = _relevantMixTextures.Items.ToArray();
			var sortedTextures = SortMixTextures(relevantTextures, _references.MixTextureOrdering);

			// Some materials have multiple textures (notably the eyes). Update them separately
			var buckets = sortedTextures.GroupBy(p => p.TargetMaterialTexture);
			foreach (var bucket in buckets)
			{
				if (!bucket.Any()) continue;
				var rt = UpdateMaterialTexture(bucket.Key, bucket);
				_cachedRenderTextures.Add(rt);
			}
		}

		RenderTexture UpdateMaterialTexture(TargetMaterialTexture materialTexture, IEnumerable<IMixTexture> applicableMixTextures)
		{
			int largestSize = applicableMixTextures.Any() ? applicableMixTextures.Max(m => m.Grayscale.width) : 1;
			// TODO: Cleanup these textures
			var renderTextures = new DoubleBufferedRenderTexture(largestSize);

			foreach (var applicableMixTexture in applicableMixTextures)
			{
				if (applicableMixTexture.Mask == null)
				{
					_references.BlitMaterial.shader = COLORIZE_SHADER;
				}
				else
				{
					_references.BlitMaterial.shader = COLORIZE_WITH_MASK_SHADER;
					_references.BlitMaterial.SetTexture(MASK_TEX_PROPERTY_ID, applicableMixTexture.Mask);
				}
				_references.BlitMaterial.SetTexture(GRAYSCALE_TEX_PROPERTY_ID, applicableMixTexture.Grayscale);
				ApplyMixTexturePropsToMaterial(_references.BlitMaterial, applicableMixTexture);
				renderTextures.Blit(_references.BlitMaterial);
			}

			var renderTexture = renderTextures.Finalize();
			if (materialTexture == TargetMaterialTexture.MainTexture)
				_material.Material.mainTexture = renderTexture;
			else if (materialTexture == TargetMaterialTexture.Outline)
				_material.Material.SetTexture(OUTLINE_PROPERTY_ID, renderTexture);
			else if (materialTexture == TargetMaterialTexture.Pupil)
				_material.Material.SetTexture(PUPIL_PROPERTY_ID, renderTexture);
			return renderTexture;
		}

		static void ApplyMixTexturePropsToMaterial(Material material, IMixTexture mixTexture)
		{
			var main = mixTexture.DefaultColorGroup.BaseDefaultColor.GetColor();
			var shade = mixTexture.DefaultColorGroup.ShadeDefaultColor.GetColor();

			material.SetColor(MAIN_COLOR_PROPERTY_ID, main);
			material.SetColor(DARK_COLOR_PROPERTY_ID, shade);
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

		void CleanupRenderTextures()
		{
			foreach (var rt in _cachedRenderTextures)
			{
				rt.Release();
				GameObject.Destroy(rt);
			}
			_cachedRenderTextures.Clear();
		}

		public void Dispose()
		{
			_reflectTextureComposite.Destroy();
			_reflectRelevantMixTextures.Destroy();
			CleanupRenderTextures();
		}
	}

	internal sealed class IndividualMaterialTexturerReferences : IDisposable
	{
		public IndividualMaterialTexturerReferences(ITextureGatherer textureGatherer, MixTextureOrdering mixTextureOrdering)
		{
			TextureGatherer = textureGatherer;
			MixTextureOrdering = mixTextureOrdering;


			BlitMaterial = new Material(IndividualMaterialTexturer.COLORIZE_SHADER);
		}

		public ITextureGatherer TextureGatherer { get; }
		public MixTextureOrdering MixTextureOrdering { get; }
		public Material BlitMaterial { get; }

		public void Dispose()
		{
			GameObject.Destroy(BlitMaterial);
		}
	}
}
