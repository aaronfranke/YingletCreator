using UnityEngine;

namespace Character.Compositor
{

	/// <summary>
	/// Values to be used by the colorization process
	/// These combined form a gradient to recolor a grayscale texture
	/// </summary>
	public interface IColorizeValues
	{
		IColorizeValuesPart Base { get; }
		IColorizeValuesPart Shade { get; }
	}



	/// <summary>
	/// Implementation of IColorizeValuesPart that can be set in the inspector
	/// </summary>
	[System.Serializable]
	public class SerializableColorizeValues : IColorizeValues
	{
		[SerializeField] SerializableColorizeValuesPart _base;
		[SerializeField] SerializableColorizeValuesPart _shade;

		public IColorizeValuesPart Base => _base;
		public IColorizeValuesPart Shade => _shade;
	}
}
