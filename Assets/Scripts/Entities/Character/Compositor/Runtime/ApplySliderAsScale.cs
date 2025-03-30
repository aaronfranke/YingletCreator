using Character.Creator;
using Character.Data;
using UnityEngine;

enum ApplySliderMode
{
    Override,
    Multiply
}

public class ApplySliderAsScale : MonoBehaviour, IApplyableCustomization
{
    [SerializeField] CharacterSliderId _sliderId;
    [SerializeField] Transform _target;
    [SerializeField] Vector3 _minSize = Vector3.one;
    [SerializeField] Vector3 _maxSize = Vector3.one;
    [SerializeField] ApplySliderMode _applyMode;
    [SerializeField] ApplySliderAsScaleAdvancedOptions _advanced;

    ICharacterCreatorDataRepository _dataRepository;

    private void Awake()
    {
        _dataRepository = GetComponentInParent<ICharacterCreatorDataRepository>();
    }

    public void Apply()
    {
        var value = GetSize();

        if (_applyMode == ApplySliderMode.Override)
        {
            _target.localScale = value;
        }
        else
        {
            _target.localScale = _target.localScale.Multiply(value);
        }

        foreach (var child in _advanced._childScaleExclusion)
        {
            child.localScale = child.localScale.Multiply(value.GetReciprocal());
        }
    }

    Vector3 GetSize()
    {
        var sliderValue = _dataRepository.GetSliderValue(_sliderId);
        var middle = _advanced._middle;
        if (sliderValue < middle)
        {
            float p = sliderValue / middle;
            return Vector3.LerpUnclamped(_minSize, Vector3.one, p);
        }
        else
        {
            float p = (sliderValue - middle) / (1 - middle);
            return Vector3.LerpUnclamped(Vector3.one, _maxSize, p);
        }
    }
}

[System.Serializable]
public class ApplySliderAsScaleAdvancedOptions
{
    [SerializeField] public float _middle = .5f;
    [SerializeField] public Transform[] _childScaleExclusion;
}
