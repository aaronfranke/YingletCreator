using UnityEngine;


namespace Character.Compositor
{
	[CreateAssetMenu(fileName = "ColorGroup", menuName = "Scriptable Objects/Character Compositor/ColorGroup")]
	public class ColorGroup : ScriptableObject
	{
		[SerializeField] SerializableColorizeValues _defaultColors;

		public IColorizeValues DefaultColors => _defaultColors;
	}
}
