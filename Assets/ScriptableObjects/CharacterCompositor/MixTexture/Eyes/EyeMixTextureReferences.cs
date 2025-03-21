using UnityEngine;

namespace CharacterCompositor
{
	[CreateAssetMenu(fileName = "EyeMixTextureReferences", menuName = "Scriptable Objects/Character Compositor/EyeMixTextureReferences")]
	public class EyeMixTextureReferences : ScriptableObject
	{
		[SerializeField] ColorGroup _defaultColorGroup;
		[SerializeField] MaterialDescription _leftMaterialDescription;
		[SerializeField] MaterialDescription _rightMaterialDescription;

		public ColorGroup DefaultColorGroup => _defaultColorGroup;
		public MaterialDescription LeftMaterialDescription => _leftMaterialDescription;
		public MaterialDescription RightMaterialDescription => _rightMaterialDescription;
	}

}