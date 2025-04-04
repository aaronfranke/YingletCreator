using Character.Creator;
using Character.Data;
using CharacterCompositor;
using Reactivity;
using System.Collections.Generic;
using UnityEngine;

public interface ICompositorMeshConstraint
{
    void Filter(ref ISet<MeshWithMaterial> meshWithMaterials);
}

public class RemoveMeshAtLowSliderValue : ReactiveBehaviour, ICompositorMeshConstraint
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


    public void Filter(ref ISet<MeshWithMaterial> meshWithMaterials)
    {
        if (_constrain.Val)
        {
            meshWithMaterials.Remove(_toRemove);
        }
    }

    bool ComputeConstrain()
    {
        var sliderValue = _dataRepository.GetSliderValue(_sliderId);
        return sliderValue < 0.01f;

    }
}
