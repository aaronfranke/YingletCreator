using Character.Data;
using Reactivity;
using UnityEngine;
using UnityEngine.UI;

namespace Character.Creator.UI
{
    public class CharacterCreatorSlider : MonoBehaviour
    {
        [SerializeField] CharacterSliderId _sliderId;
        private ICustomizationSelectedDataRepository _dataRepo;
        private Slider _slider;

        private void Awake()
        {
            _dataRepo = this.GetComponentInParent<ICustomizationSelectedDataRepository>();
            _slider = this.GetComponentInChildren<Slider>();
            _slider.onValueChanged.AddListener(Slider_OnValueChanged);
        }

        private void OnDestroy()
        {
            _slider.onValueChanged.RemoveListener(Slider_OnValueChanged);
        }

        private void Slider_OnValueChanged(float arg0)
        {
            ObservableDictUtils<CharacterSliderId, float>.SetOrUpdate(_dataRepo.CustomizationData.SliderData.SliderValues, _sliderId, arg0);
        }
    }
}