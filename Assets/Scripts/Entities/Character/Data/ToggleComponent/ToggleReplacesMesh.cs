using Character.Compositor;
using UnityEngine;

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
		[SerializeField] MeshWithMaterial _toReplace;
		[SerializeField] MeshWithMaterial _replacement;

		public MeshWithMaterial ToReplace => _toReplace;
		public MeshWithMaterial Replacement => _replacement;
	}
}