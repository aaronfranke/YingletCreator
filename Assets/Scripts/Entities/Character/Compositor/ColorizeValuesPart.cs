using UnityEngine;

namespace Character.Compositor
{
	/// <summary>
	/// Partial values to be used by the colorization process
	/// This represents just one half of the two HSL colors that make up the gradient
	/// </summary>
	public interface IColorizeValuesPart
	{
		float Hue { get; }
		float Saturation { get; }
		float Value { get; }
	}

	/// <summary>
	/// Implementation of IColorizeValuesPart that can be set in the inspector
	/// </summary>
	[System.Serializable]
	public class SerializableColorizeValuesPart : IColorizeValuesPart
	{
		[SerializeField][Range(0, 1)] float _hue = 0.08055f;
		[SerializeField][Range(0, 1)] float _saturation = .40f;
		[SerializeField][Range(0, 1)] float _value = .84f;

		public float Hue => _hue;

		public float Saturation => _saturation;

		public float Value => _value;

		public SerializableColorizeValuesPart(IColorizeValuesPart values)
		{
			_hue = values.Hue;
			_saturation = values.Saturation;
			_value = values.Value;
		}

		public void Set(Color color)
		{
			Color.RGBToHSV(color, out _hue, out _saturation, out _value);
		}
	}
	public static class ColorizeValuesExtensionMethods
	{
		public static Color GetColor(this IColorizeValuesPart c)
		{
			return Color.HSVToRGB(Wrap01(c.Hue), Clamp01(c.Saturation), Clamp01(c.Value));
		}
		static float Wrap01(float input)
		{
			return ((input % 1f) + 1f) % 1f;
		}
		static float Clamp01(float input)
		{
			return Mathf.Clamp01(input);
		}
	}
}
