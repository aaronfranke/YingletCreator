using CharacterCompositor;
using System.Collections.Generic;
using UnityEngine;

public class MeshDefinition_StaticAdditions : MonoBehaviour, IMeshDefinitionMutator
{
	[SerializeField] MeshWithMaterial[] _meshes;
	public void Mutate(ref ISet<MeshWithMaterial> set)
	{
		foreach (var mesh in _meshes)
		{
			set.Add(mesh);
		}
	}
}
