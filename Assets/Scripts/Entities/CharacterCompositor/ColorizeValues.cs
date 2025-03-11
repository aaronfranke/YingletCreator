using Unity.VisualScripting;
using UnityEngine;

namespace CharacterCompositor
{
	/// <summary>
	/// Values to be used by the colorization process for textures
	/// This adjusts hue, mult, saturation, and contrast
	/// </summary>
	public interface IColorizeValues
	{
		float HueShift { get; }
		float Multiplication { get; }
		float Contrast { get; }
		float Saturation { get; }
	}

	/// <summary>
	/// Implementation of IColorizeValues that can be set in the inspector
	/// </summary>
	[System.Serializable]
	public class SerializableColorizeValues : IColorizeValues
	{
		[SerializeField][Range(0, 360)] float _hueShift = 0;
		[SerializeField][Range(0, 2)] float _multiplication = 1;
		[SerializeField][Range(0, 3)] float _contrast = 1;
		[SerializeField][Range(0, 2)] float _saturation = 1;

		public float HueShift => _hueShift;
		public float Multiplication => _multiplication;
		public float Contrast => _contrast;
		public float Saturation => _saturation;
	}
}
