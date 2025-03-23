using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Quick and dumb script i made to test out scaling
/// </summary>
public class ThiccSlider : MonoBehaviour
{
    [SerializeField] Slider _slider;
    [SerializeField] float _maxBoobSize = 3;
    [SerializeField] float _maxThighSize = 3;

    [SerializeField] Transform[] _boobs;
    [SerializeField] Transform[] _thighs;

    [SerializeField] float _minValue = -0.14f;
    [SerializeField] GameObject[] _disableOnMin;

    void Update()
    {
        float boobScale = Mathf.LerpUnclamped(1, _maxBoobSize, _slider.value);
        foreach (var boob in _boobs)
        {
            boob.transform.localScale = Vector3.one * boobScale;
        }

        float _thighScale = Mathf.LerpUnclamped(1, _maxThighSize, _slider.value);
        foreach (var thigh in _thighs)
        {
            thigh.transform.localScale = new Vector3(_thighScale, 1, 1);
        }

        foreach (var disableOnMin in _disableOnMin)
        {
            disableOnMin.SetActive(_slider.value > _minValue);
        }
    }
}
