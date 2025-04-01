using UnityEngine;

namespace CharacterCompositor
{
    [System.Serializable]
    public sealed class ColoredEyeMixTextureReferences
    {
        [SerializeField] ColorGroup _defaultColorGroup;
        [SerializeField] Color _contrastMidpointColor;

        public ColorGroup DefaultColorGroup => _defaultColorGroup;
        public Color ContrastMidpointColor => _contrastMidpointColor;
    }

    [CreateAssetMenu(fileName = "EyeMixTextureReferences", menuName = "Scriptable Objects/Character Compositor/EyeMixTextureReferences")]
    public class EyeMixTextureReferences : ScriptableObject
    {
        [SerializeField] ColoredEyeMixTextureReferences _fill;
        [SerializeField] ColoredEyeMixTextureReferences _eyelid;
        [SerializeField] MaterialDescription _leftMaterialDescription;
        [SerializeField] MaterialDescription _rightMaterialDescription;

        public ColoredEyeMixTextureReferences Fill => _fill;
        public ColoredEyeMixTextureReferences Eyelid => _eyelid;

        public MaterialDescription LeftMaterialDescription => _leftMaterialDescription;
        public MaterialDescription RightMaterialDescription => _rightMaterialDescription;
    }

}