using UnityEngine;

namespace Character.Compositor
{
	/// <summary>
	/// Values to be used by the colorization process for textures
	/// This adjusts hue, mult, saturation, and contrast
	/// </summary>
	public interface IColorizeValues
	{
		float Hue { get; }
		float Saturation { get; }
		float Value { get; }
	}

	/// <summary>
	/// Implementation of IColorizeValues that can be set in the inspector
	/// </summary>
	[System.Serializable]
	public class SerializableColorizeValues : IColorizeValues
	{
		[SerializeField][Range(0, 1)] float _hue = 0.08055f;
		[SerializeField][Range(0, 1)] float _saturation = .40f;
		[SerializeField][Range(0, 1)] float _value = .84f;

		public float Hue => _hue;

		public float Saturation => _saturation;

		public float Value => _value;

		public void Set(Color color)
		{
			Color.RGBToHSV(color, out _hue, out _saturation, out _value);
		}
	}
	public static class ColorizeValuesExtensionMethods
	{
		public static Color GetColor(this IColorizeValues c)
		{
			return Color.HSVToRGB(c.Hue, c.Saturation, c.Value);
		}
	}
}
