using Character.Creator;
using Character.Data;
using UnityEngine;


public class ApplySliderAsScale : MonoBehaviour
{
    [SerializeField] CharacterSliderId _sliderId;
    [SerializeField] Vector3 _minSize = Vector3.one;
    [SerializeField] Vector3 _maxSize = Vector3.one;

    ICharacterCreatorDataRepository _dataRepository;

    private void Awake()
    {
        _dataRepository = GetComponentInParent<ICharacterCreatorDataRepository>();
    }

    // Do this every frame in late-update so it happens after the animator
    private void LateUpdate()
    {
        this.transform.localScale = Vector3.LerpUnclamped(_minSize, _maxSize, GetValue());
    }

    float GetValue()
    {

        if (_dataRepository.CustomizationData.SliderData.SliderValues.TryGetValue(_sliderId, out float value))
        {
            return value;
        }
        return 0.5f;
    }
}
