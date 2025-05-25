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
		[SerializeField] Texture2D _fill;
		[SerializeField] Texture2D _eyelid;
		[SerializeField] Texture2D _pupil;

		public Texture2D Fill => _fill;
		public Texture2D Eyelid => _eyelid;

		public void EditorSetTextures(Texture2D fill, Texture2D eyelid, Texture2D pupil)
		{
			_fill = fill;
			_eyelid = eyelid;
			_pupil = pupil;
		}


		public IEnumerable<IMixTexture> GenerateMixTextures(EyeMixTextureReferences references)
		{
			var mixtextures = new List<EyeMixTexture>();
			mixtextures.Add(new EyeMixTexture(_eyelid, references, references.Eyelid, true));
			mixtextures.Add(new EyeMixTexture(_eyelid, references, references.Eyelid, false));
			mixtextures.Add(new EyeMixTexture(_fill, references, references.Fill, true));
			mixtextures.Add(new EyeMixTexture(_fill, references, references.Fill, false));
			mixtextures.Add(new EyeMixTexture(_pupil, references, references.Pupil, true));
			mixtextures.Add(new EyeMixTexture(_pupil, references, references.Pupil, false));
			return mixtextures;
		}
	}
}