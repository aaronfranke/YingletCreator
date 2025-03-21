using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterCompositor
{

	public interface IEyeMixTextures
	{
		string name { get; }
		public Color ContrastMidpointColor { get; }
		public Texture2D Fill { get; }
		IEnumerable<IMixTexture> GenerateMixTextures(EyeMixTextureReferences references);
		void ApplyEyeProperties(IReadOnlyDictionary<MaterialDescription, Material> materialMapping, EyeMixTextureReferences references);
	}

	[CreateAssetMenu(fileName = "EyeMixTextures", menuName = "Scriptable Objects/Character Compositor/EyeMixTextures")]
	public class EyeMixTextures : ScriptableObject, IEyeMixTextures
	{
		[SerializeField][ColorUsage(false)] Color _contrastMidpointColor = Color.gray;
		
		// The following are public only because UpdateEyeAsset wants to set them
		public Texture2D _fill;
		public Texture2D _outline;
		public Texture2D _pupil;

		public Color ContrastMidpointColor => _contrastMidpointColor;
		public Texture2D Fill => _fill;

		public IEnumerable<IMixTexture> GenerateMixTextures(EyeMixTextureReferences references)
		{
			return new[] {
				new EyeMixTexture(this, references, true),
				new EyeMixTexture(this, references, false),
				// TODO eventually: add support for eyelids here
			};
		}

		public void ApplyEyeProperties(IReadOnlyDictionary<MaterialDescription, Material> materialMapping, EyeMixTextureReferences references)
		{
			Apply(references.LeftMaterialDescription);
			Apply(references.RightMaterialDescription);

			void Apply(MaterialDescription materialDescription)
			{
				if (materialMapping.TryGetValue(materialDescription, out Material material))
				{
					material.SetTexture("_Outline", _outline);
					material.SetTexture("_Pupil", _pupil);
				}
				else
				{
					Debug.LogWarning($"Material mapping did not contain expected MaterialDescription {materialDescription.name}");
				}
			}
		}
	}
}