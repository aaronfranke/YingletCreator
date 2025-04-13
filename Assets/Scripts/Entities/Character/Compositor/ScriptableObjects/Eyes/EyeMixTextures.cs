using System.Collections.Generic;
using UnityEngine;

namespace Character.Compositor
{

    public interface IEyeMixTextures
    {
        string name { get; }
        public Texture2D Fill { get; }
        public Texture2D Eyelid { get; }
        IEnumerable<IMixTexture> GenerateMixTextures(EyeMixTextureReferences references);
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


        public IEnumerable<IMixTexture> GenerateMixTextures(EyeMixTextureReferences references)
        {
            var mixtextures = new List<EyeMixTexture>();
            if (_eyelid != null)
            {
                mixtextures.Add(new EyeMixTexture(_eyelid, references, references.Eyelid, true));
                mixtextures.Add(new EyeMixTexture(_eyelid, references, references.Eyelid, false));
            }
            mixtextures.Add(new EyeMixTexture(_fill, references, references.Fill, true));
            mixtextures.Add(new EyeMixTexture(_fill, references, references.Fill, false));
            mixtextures.Add(new EyeMixTexture(_outline, references, references.Outline, true));
            mixtextures.Add(new EyeMixTexture(_outline, references, references.Outline, false));
            mixtextures.Add(new EyeMixTexture(_pupil, references, references.Pupil, true));
            mixtextures.Add(new EyeMixTexture(_pupil, references, references.Pupil, false));
            return mixtextures;
        }
    }
}