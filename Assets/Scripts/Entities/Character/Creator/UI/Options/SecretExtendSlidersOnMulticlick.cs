using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace Character.Creator.UI
{
    public class SecretExtendSlidersOnMultiClick : MonoBehaviour, IPointerClickHandler
    {
        string _originalText;
        const int SWAP_CLICKS = 5;
        int _clicks = 0;
        private TextMeshProUGUI _text;

        void Awake()
        {
            _text = this.GetComponent<TextMeshProUGUI>();
            _originalText = _text.text;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _clicks += 1;
            if (_clicks == SWAP_CLICKS)
            {
                SetSliderRanges(-3, 4);
                _text.text = "GIGA SLIDERS";
            }
            else if (_clicks > SWAP_CLICKS)
            {
                SetSliderRanges(0, 1);
                _clicks = 0;
                _text.text = _originalText;
            }
        }

        void SetSliderRanges(float min, float max)
        {
            var canvas = this.GetComponentInParent<Canvas>();
            var sliders = canvas.GetComponentsInChildren<Slider>();
            foreach (var slider in sliders)
            {
                slider.minValue = min;
                slider.maxValue = max;
            }
        }
    }
}
