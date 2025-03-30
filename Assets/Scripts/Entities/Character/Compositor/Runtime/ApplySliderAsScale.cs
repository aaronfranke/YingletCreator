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
    [SerializeField] float _middle = .5f;
    [SerializeField] ApplySliderMode _applyMode;

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
    }

    Vector3 GetSize()
    {
        var sliderValue = GetSliderValue();
        if (sliderValue < _middle)
        {
            float p = sliderValue / _middle;
            return Vector3.LerpUnclamped(_minSize, Vector3.one, p);
        }
        else
        {
            float p = (sliderValue - _middle) / (1 - _middle);
            return Vector3.LerpUnclamped(Vector3.one, _maxSize, p);
        }
    }

    float GetSliderValue()
    {

        if (_dataRepository.CustomizationData.SliderData.SliderValues.TryGetValue(_sliderId, out float value))
        {
            return value;
        }
        return 0.5f;
    }
}
