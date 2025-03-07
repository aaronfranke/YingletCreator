using UnityEngine;


namespace CharacterCompositor
{
	[CreateAssetMenu(fileName = "ColorGroup", menuName = "Scriptable Objects/Character Compositor/ColorGroup")]
	public class ColorGroup : ScriptableObject
	{
		[SerializeField] SerializableColorizeValues _defaultColorValues;

		public IColorizeValues DefaultColorValues => _defaultColorValues;
    }
}
