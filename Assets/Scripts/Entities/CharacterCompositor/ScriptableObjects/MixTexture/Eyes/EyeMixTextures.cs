using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterCompositor
{

	[CreateAssetMenu(fileName = "EyeMixTextures", menuName = "Scriptable Objects/Character Compositor/EyeMixTextures")]
	public class EyeMixTextures : ScriptableObject
	{
		[SerializeField][ColorUsage(false)] Color _contrastMidpointColor = Color.gray;
		[SerializeField] Texture2D _fill;
		[SerializeField] Texture2D _outline;
		[SerializeField] Texture2D _pupil;

		public Color ContrastMidpointColor => _contrastMidpointColor;
		public Texture2D Fill => _fill;
		public Texture2D Outline => _outline;
		public Texture2D Pupil => _pupil;

		public IEnumerable<IMixTexture> GenerateMixTextures(EyeMixTextureReferences references)
		{
			return new[] {
				new EyeMixTexture(this, references, true),
				new EyeMixTexture(this, references, false),
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