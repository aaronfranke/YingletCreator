using UnityEngine;


namespace CharacterCompositor
{
	public interface IColorGroup
	{
		IColorizeValues DefaultColorValues { get; }
	}

	[CreateAssetMenu(fileName = "ColorGroup", menuName = "Scriptable Objects/Character Compositor/ColorGroup")]
	public class ColorGroup : ScriptableObject, IColorGroup
	{
		[SerializeField] SerializableColorizeValues _defaultColorValues;

		public IColorizeValues DefaultColorValues => _defaultColorValues;
    }
}
