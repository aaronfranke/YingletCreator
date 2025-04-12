using Character.Data;
using UnityEngine;

namespace CharacterCompositor
{
	[System.Serializable]
	public sealed class EyeIndividualMixTextureReferences
	{
		[SerializeField] ColorGroup _defaultColorGroup;
		[SerializeField] ReColorId _leftReColorId;
		[SerializeField] ReColorId _rightReColorId;
		[SerializeField] TargetMaterialTexture _targetMaterialTexture;

		public ColorGroup DefaultColorGroup => _defaultColorGroup;
		public ReColorId LeftReColorId => _leftReColorId;
		public ReColorId RightReColorId => _rightReColorId;
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
		[SerializeField] ReColorId _leftEyeColorId;
		[SerializeField] ReColorId _rightEyeColorId;
		[SerializeField] ReColorId _eyelidColorId;

		public EyeIndividualMixTextureReferences Fill => _fill;
		public EyeIndividualMixTextureReferences Eyelid => _eyelid;
		public EyeIndividualMixTextureReferences Pupil => _pupil;
		public EyeIndividualMixTextureReferences Outline => _outline;

		public MaterialDescription LeftMaterialDescription => _leftMaterialDescription;
		public MaterialDescription RightMaterialDescription => _rightMaterialDescription;
	}

}