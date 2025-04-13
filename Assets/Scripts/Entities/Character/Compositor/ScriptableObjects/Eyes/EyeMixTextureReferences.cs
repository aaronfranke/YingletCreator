using Character.Data;
using UnityEngine;

namespace Character.Compositor
{
	[System.Serializable]
	public sealed class EyeIndividualMixTextureReferences
	{
		[SerializeField] ReColorId _leftReColorId;
		[SerializeField] ReColorId _rightReColorId;
		[SerializeField] TargetMaterialTexture _targetMaterialTexture;

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
		[SerializeField] ReColorId _eyelidColorId;

		public EyeIndividualMixTextureReferences Fill => _fill;
		public EyeIndividualMixTextureReferences Eyelid => _eyelid;
		public EyeIndividualMixTextureReferences Pupil => _pupil;
		public EyeIndividualMixTextureReferences Outline => _outline;

		public MaterialDescription LeftMaterialDescription => _leftMaterialDescription;
		public MaterialDescription RightMaterialDescription => _rightMaterialDescription;
	}

}