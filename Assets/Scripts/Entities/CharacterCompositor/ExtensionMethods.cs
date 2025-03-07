using UnityEngine;

namespace CharacterCompositor
{
	public static class ExtensionMethods
	{
		public static void SetColorizeParams(this Material material, IColorizeValues values)
		{
			material.SetFloat("_HueShift", values.HueShift);
			material.SetFloat("_Multiplication", values.Multiplication);
			material.SetFloat("_Contrast", values.Contrast);
			material.SetFloat("_Saturation", values.Saturation);
		}
	}
}
