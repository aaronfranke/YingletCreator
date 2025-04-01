using System.Collections.Generic;
using UnityEngine;

namespace CharacterCompositor
{

    public interface IEyeMixTextures
    {
        string name { get; }
        public Texture2D Fill { get; }
        public Texture2D Eyelid { get; }
        IEnumerable<IMixTexture> GenerateMixTextures(EyeMixTextureReferences references);
        void ApplyEyeProperties(IReadOnlyDictionary<MaterialDescription, Material> materialMapping, EyeMixTextureReferences references);
    }

    [CreateAssetMenu(fileName = "EyeMixTextures", menuName = "Scriptable Objects/Character Compositor/EyeMixTextures")]
    public class EyeMixTextures : ScriptableObject, IEyeMixTextures
    {
        // The following are public only because UpdateEyeAsset wants to set them
        public Texture2D _fill;
        public Texture2D _eyelid;
        public Texture2D _outline;
        public Texture2D _pupil;

        public Texture2D Fill => _fill;
        public Texture2D Eyelid => _eyelid;

        static readonly int OUTLINE_PROPERTY_ID = Shader.PropertyToID("_Outline");
        static readonly int PUPIL_PROPERTY_ID = Shader.PropertyToID("_Pupil");


        public IEnumerable<IMixTexture> GenerateMixTextures(EyeMixTextureReferences references)
        {
            var mixtextures = new List<EyeMixTexture>();
            if (_eyelid != null)
            {
                mixtextures.Add(new EyeMixTexture(this, references, true, true));
                mixtextures.Add(new EyeMixTexture(this, references, false, true));
            }
            mixtextures.Add(new EyeMixTexture(this, references, true, false));
            mixtextures.Add(new EyeMixTexture(this, references, false, false));
            return mixtextures;
        }

        public void ApplyEyeProperties(IReadOnlyDictionary<MaterialDescription, Material> materialMapping, EyeMixTextureReferences references)
        {
            Apply(references.LeftMaterialDescription);
            Apply(references.RightMaterialDescription);

            void Apply(MaterialDescription materialDescription)
            {
                if (materialMapping.TryGetValue(materialDescription, out Material material))
                {
                    material.SetTexture(OUTLINE_PROPERTY_ID, _outline);
                    material.SetTexture(PUPIL_PROPERTY_ID, _pupil);
                }
                else
                {
                    Debug.LogWarning($"Material mapping did not contain expected MaterialDescription {materialDescription.name}");
                }
            }
        }
    }
}