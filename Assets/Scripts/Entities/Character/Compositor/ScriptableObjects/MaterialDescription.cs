using UnityEngine;
namespace Character.Compositor
{

	/// <summary>
	/// Details that describe how a material should be constructed
	/// This is used so separate meshes can share a common material
	/// </summary>

	[CreateAssetMenu(fileName = "MaterialDescription", menuName = "Scriptable Objects/Character Compositor/MaterialDescription")]
	public class MaterialDescription : ScriptableObject
	{
		[SerializeField] Material _referenceMaterial;
		public Material ReferenceMaterial => _referenceMaterial;
	}
}
