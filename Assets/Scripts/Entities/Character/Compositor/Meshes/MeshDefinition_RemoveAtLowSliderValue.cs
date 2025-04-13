using Character.Creator;
using Character.Data;
using Reactivity;
using System.Collections.Generic;
using UnityEngine;

namespace Character.Compositor
{

	public class MeshDefinition_RemoveAtLowSliderValue : ReactiveBehaviour, IMeshDefinitionMutator
	{
		[SerializeField] MeshWithMaterial _toRemove;
		[SerializeField] CharacterSliderId _sliderId;
		[SerializeField] float _minimumValue;

		ICustomizationSelectedDataRepository _dataRepository;
		private Computed<bool> _constrain;

		private void Awake()
		{
			_dataRepository = GetComponentInParent<ICustomizationSelectedDataRepository>();
			_constrain = CreateComputed(ComputeConstrain);
		}

		bool ComputeConstrain()
		{
			var sliderValue = _dataRepository.GetSliderValue(_sliderId);
			return sliderValue < 0.01f;
		}

		public void Mutate(ref ISet<MeshWithMaterial> meshes)
		{
			if (_constrain.Val)
			{
				meshes.Remove(_toRemove);
			}
		}
	}
}
