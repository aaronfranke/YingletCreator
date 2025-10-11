using UnityEngine;

namespace Character.Data
{
    // Used for certain hairstyles that don't really support hair length
    [CreateAssetMenu(fileName = "ToggleDisablesSlider", menuName = "Scriptable Objects/Character Data/ToggleCompnents/ToggleDisablesSlider")]
    public class ToggleDisablesSlider : CharacterToggleComponent
    {
        [SerializeField] CharacterSliderId _sliderToDisable;
        public CharacterSliderId SliderToDisable => _sliderToDisable;
    }
}