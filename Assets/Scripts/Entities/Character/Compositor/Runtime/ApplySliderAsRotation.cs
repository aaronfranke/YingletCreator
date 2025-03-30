using Character.Creator;
using Character.Data;
using UnityEngine;

public class ApplySliderAsRotation : MonoBehaviour, IApplyableCustomization
{
    [SerializeField] CharacterSliderId _sliderId;
    [SerializeField] Transform _target;
    [SerializeField] Vector3 _eulerAngles;
    [SerializeField] AnimationCurve _applyAmountBySliderVal;

    ICharacterCreatorDataRepository _dataRepository;

    private void Awake()
    {
        _dataRepository = GetComponentInParent<ICharacterCreatorDataRepository>();
    }

    public void Apply()
    {
        var sliderValue = _dataRepository.GetSliderValue(_sliderId);

        var p = _applyAmountBySliderVal.Evaluate(sliderValue);
        if (p < 0.01f) return; // Don't apply if it won't be relevant

        _target.transform.localRotation = _target.transform.localRotation * Quaternion.Euler(_eulerAngles * p);
    }
}
