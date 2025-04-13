using UnityEngine;


namespace Character.Compositor
{
	[CreateAssetMenu(fileName = "ColorGroup", menuName = "Scriptable Objects/Character Compositor/ColorGroup")]
	public class ColorGroup : ScriptableObject
	{
		[SerializeField] SerializableColorizeValues _baseDefaultColor;
		[SerializeField] SerializableColorizeValues _shadeDefaultColor;

		public IColorizeValues BaseDefaultColor => _baseDefaultColor;
		public IColorizeValues ShadeDefaultColor => _shadeDefaultColor;
	}
}
