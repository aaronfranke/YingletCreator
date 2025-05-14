using UnityEngine;

namespace Character.Compositor
{
	[CreateAssetMenu(fileName = "MixTextureWithMatTarget", menuName = "Scriptable Objects/Character Compositor/MixTextureWithMatTarget")]
	public class MixTextureWithMatTarget : MixTexture
	{
		[SerializeField] TargetMaterialTexture _targetMatTexture;
		public override TargetMaterialTexture TargetMaterialTexture => _targetMatTexture;
	}
}