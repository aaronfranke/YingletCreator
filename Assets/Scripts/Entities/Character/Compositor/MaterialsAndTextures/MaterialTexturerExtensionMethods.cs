using UnityEngine;

namespace Character.Compositor
{
	internal static class MaterialTexturerExtensionMethods
	{
		static readonly int EYELID_PROPERTY_ID = Shader.PropertyToID("_Eyelid");
		static readonly int PUPIL_PROPERTY_ID = Shader.PropertyToID("_Pupil");
		static readonly int MOUTH_PROPERTY_ID = Shader.PropertyToID("_Mouth");
		static readonly int MOUTH_MASK_PROPERTY_ID = Shader.PropertyToID("_MouthMask");

		public static void ApplyTexture(this Material material, Texture texture, TargetMaterialTexture materialTexture)
		{
			if (materialTexture == TargetMaterialTexture.MainTexture)
				material.mainTexture = texture;
			else if (materialTexture == TargetMaterialTexture.Eyelid)
				material.SetTexture(EYELID_PROPERTY_ID, texture);
			else if (materialTexture == TargetMaterialTexture.Pupil)
				material.SetTexture(PUPIL_PROPERTY_ID, texture);
			else if (materialTexture == TargetMaterialTexture.Mouth)
				material.SetTexture(MOUTH_PROPERTY_ID, texture);
			else if (materialTexture == TargetMaterialTexture.MouthMask)
				material.SetTexture(MOUTH_MASK_PROPERTY_ID, texture);
		}
	}
}
