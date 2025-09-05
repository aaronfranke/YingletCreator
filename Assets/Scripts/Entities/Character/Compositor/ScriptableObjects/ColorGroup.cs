using UnityEngine;


namespace Character.Compositor
{
	[CreateAssetMenu(fileName = "ColorGroup", menuName = "Scriptable Objects/Character Compositor/ColorGroup")]
	public class ColorGroup : ScriptableObject
	{
		[SerializeField] SerializableColorizeValues _defaultColors;

		[SerializeField]
		[Tooltip("If true, when a RecolorId is first added to the toggles, it will attempt to copy the color of something else in this same color group")]
		bool _autoColorWithGroup = false;

		public IColorizeValues DefaultColors => _defaultColors;
		public bool AutoColorWithGroup => _autoColorWithGroup;
	}
}
