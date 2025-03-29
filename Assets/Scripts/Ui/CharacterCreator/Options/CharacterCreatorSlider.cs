using Character.Creator;
using Character.Data;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCreatorSlider : MonoBehaviour
{
    [SerializeField] CharacterSliderId _sliderId;
    private ICharacterCreatorDataRepository _dataRepo;
    private Slider _slider;

    private void Awake()
    {
        _dataRepo = this.GetComponentInParent<ICharacterCreatorDataRepository>();
        _slider = this.GetComponent<Slider>();
        _slider.onValueChanged.AddListener(Slider_OnValueChanged);
    }

    private void OnDestroy()
    {
        _slider.onValueChanged.RemoveListener(Slider_OnValueChanged);
    }

    private void Slider_OnValueChanged(float arg0)
    {
        _dataRepo.CustomizationData.SliderData.SliderValues[_sliderId] = arg0;
    }
}
