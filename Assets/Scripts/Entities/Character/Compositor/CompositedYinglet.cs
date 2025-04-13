using Reactivity;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Character.Compositor
{
	public interface ICompositedYinglet
	{
		event Action OnSkinnedMeshRenderersRegenerated;
	}
	public class CompositedYinglet : ReactiveBehaviour, ICompositedYinglet
	{
		[SerializeField] Transform _rigRoot;
		[SerializeField] MeshWithMaterial[] _meshesWithMaterials;
		[SerializeField] MixTexture[] _mixTextures;
		[SerializeField] EyeMixTextures _eyeMixTexture;
		[SerializeField] EyeMixTextureReferences _eyeMixTextureReferences;
		[SerializeField] MixTextureOrdering _mixTextureOrdering;

		IReadOnlyDictionary<MeshWithMaterial, GameObject> _lastMeshMapping;
		IReadOnlyDictionary<MaterialDescription, Material> _lastMaterialMapping;
		private Dictionary<string, Transform> _boneMap;

		public event Action OnSkinnedMeshRenderersRegenerated = delegate { };

		void Awake()
		{
			if (!this.isActiveAndEnabled) return;

			AddReflector(Composite);
		}
		public void Composite()
		{
			Clear();
			ISet<MeshWithMaterial> filteredMeshesWithMaterials = new HashSet<MeshWithMaterial>(_meshesWithMaterials);
			if (_boneMap == null) _boneMap = MeshUtilities.GetBoneMap(_rigRoot);
			_lastMeshMapping = MeshUtilities.GenerateMeshes(_rigRoot, _boneMap, filteredMeshesWithMaterials);
			OnSkinnedMeshRenderersRegenerated();
			_lastMaterialMapping = MaterialUtilities.ApplyMaterialsToMeshes(_lastMeshMapping);
			UpdateColorGroup();
		}

		public void Clear()
		{
			MeshUtilities.ClearMeshes(_rigRoot);
		}

		public void UpdateColorGroup()
		{
			if (_lastMaterialMapping == null) return;
			// Optimization opportunity: Pass in the color group and use it to filter materials that need updating
			IEnumerable<IMixTexture> mixTextures = _mixTextures.Concat(_eyeMixTexture.GenerateMixTextures(_eyeMixTextureReferences)).ToArray();
			TextureUtilities.UpdateMaterialsWithTextures(_lastMaterialMapping, mixTextures, _mixTextureOrdering);
		}
	}
}
