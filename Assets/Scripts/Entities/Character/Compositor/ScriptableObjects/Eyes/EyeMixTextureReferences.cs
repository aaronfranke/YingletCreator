using Character.Data;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Character.Compositor
{

	[CreateAssetMenu(fileName = "EyeMixTextureReferences", menuName = "Scriptable Objects/Character Compositor/EyeMixTextureReferences")]
	public class EyeMixTextureReferences : ScriptableObject
	{
		[SerializeField] ReColorId _leftFillReColorId;  // TTODO
		[SerializeField] ReColorId _rightFillReColorId;  // TTODO
		[SerializeField] ReColorId _coloredEylidReColorId;  // TTODO

		[SerializeField] TargetMaterialTexture _fillTarget;
		[SerializeField] TargetMaterialTexture _eyelidTarget;
		[SerializeField] TargetMaterialTexture _pupilTarget;
		[SerializeField] AssetReferenceT<MaterialDescription> _leftMaterialDescriptionReference;
		[SerializeField] AssetReferenceT<MaterialDescription> _rightMaterialDescriptionReference;

		public ReColorId LeftFillReColorId => _leftFillReColorId;
		public ReColorId RightFillReColorId => _rightFillReColorId;
		public ReColorId ColoredEylidReColorId => _coloredEylidReColorId;

		public TargetMaterialTexture FillTarget => _fillTarget;
		public TargetMaterialTexture EyelidTarget => _eyelidTarget;
		public TargetMaterialTexture PupilTarget => _pupilTarget;

		public MaterialDescription LeftMaterialDescription => _leftMaterialDescriptionReference.LoadSync();
		public MaterialDescription RightMaterialDescription => _rightMaterialDescriptionReference.LoadSync();
	}

}