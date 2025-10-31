using Character.Compositor;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Character.Data
{
	public interface IToggleReplacesMesh
	{
		MeshWithMaterial ToReplace { get; }
		MeshWithMaterial Replacement { get; }
	}
	[CreateAssetMenu(fileName = "ToggleReplacesMesh", menuName = "Scriptable Objects/Character Data/ToggleCompnents/ToggleReplacesMesh")]
	public class ToggleReplacesMesh : CharacterToggleComponent, IToggleReplacesMesh
	{
		[SerializeField] AssetReferenceT<MeshWithMaterial> _toReplaceReference;
		[SerializeField] AssetReferenceT<MeshWithMaterial> _replacementReference;

		public MeshWithMaterial ToReplace => _toReplaceReference.LoadSync();
		public MeshWithMaterial Replacement => _replacementReference.LoadSync();
	}
}