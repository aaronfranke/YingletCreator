using Character.Data;
using UnityEngine;

namespace Character.Compositor
{

	[CreateAssetMenu(fileName = "EyeMixTextureReferences", menuName = "Scriptable Objects/Character Compositor/EyeMixTextureReferences")]
	public class EyeMixTextureReferences : ScriptableObject
	{
		[SerializeField] ReColorId _leftFillReColorId;
		[SerializeField] ReColorId _rightFillReColorId;
		[SerializeField] ReColorId _coloredEylidReColorId;

		[SerializeField] TargetMaterialTexture _fillTarget;
		[SerializeField] TargetMaterialTexture _eyelidTarget;
		[SerializeField] TargetMaterialTexture _pupilTarget;
		[SerializeField] MaterialDescription _leftMaterialDescription;
		[SerializeField] MaterialDescription _rightMaterialDescription;

		public ReColorId LeftFillReColorId => _leftFillReColorId;
		public ReColorId RightFillReColorId => _rightFillReColorId;
		public ReColorId ColoredEylidReColorId => _coloredEylidReColorId;

		public TargetMaterialTexture FillTarget => _fillTarget;
		public TargetMaterialTexture EyelidTarget => _eyelidTarget;
		public TargetMaterialTexture PupilTarget => _pupilTarget;

		public MaterialDescription LeftMaterialDescription => _leftMaterialDescription;
		public MaterialDescription RightMaterialDescription => _rightMaterialDescription;
	}

}