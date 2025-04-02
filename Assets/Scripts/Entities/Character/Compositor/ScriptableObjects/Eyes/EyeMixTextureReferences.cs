using UnityEngine;

namespace CharacterCompositor
{
    [System.Serializable]
    public sealed class EyeIndividualMixTextureReferences
    {
        [SerializeField] ColorGroup _defaultColorGroup;
        [SerializeField] Color _contrastMidpointColor;
        [SerializeField] TargetMaterialTexture _targetMaterialTexture;

        public ColorGroup DefaultColorGroup => _defaultColorGroup;
        public Color ContrastMidpointColor => _contrastMidpointColor;
        public TargetMaterialTexture TargetMaterialTexture => _targetMaterialTexture;
    }

    [CreateAssetMenu(fileName = "EyeMixTextureReferences", menuName = "Scriptable Objects/Character Compositor/EyeMixTextureReferences")]
    public class EyeMixTextureReferences : ScriptableObject
    {
        [SerializeField] EyeIndividualMixTextureReferences _fill;
        [SerializeField] EyeIndividualMixTextureReferences _eyelid;
        [SerializeField] EyeIndividualMixTextureReferences _pupil;
        [SerializeField] EyeIndividualMixTextureReferences _outline;
        [SerializeField] MaterialDescription _leftMaterialDescription;
        [SerializeField] MaterialDescription _rightMaterialDescription;

        public EyeIndividualMixTextureReferences Fill => _fill;
        public EyeIndividualMixTextureReferences Eyelid => _eyelid;
        public EyeIndividualMixTextureReferences Pupil => _pupil;
        public EyeIndividualMixTextureReferences Outline => _outline;

        public MaterialDescription LeftMaterialDescription => _leftMaterialDescription;
        public MaterialDescription RightMaterialDescription => _rightMaterialDescription;
    }

}