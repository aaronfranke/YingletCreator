using Reactivity;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SliderTint : ReactiveBehaviour
{
    [SerializeField] Color _hoverColor;
    [SerializeField] EaseSettings _easeSettings;
    [SerializeField] Image[] _images;
    private Color _originalColor;
    private IHoverableDetector _hoverable;
    private Coroutine _transitionCoroutine;

    private void Awake()
    {
        _originalColor = _images.First().color;
        _hoverable = this.GetComponent<IHoverableDetector>();
    }

    void Start()
    {
        AddReflector(Reflect);
    }

    private void Reflect()
    {
        Color from = _images.First().color;
        Color to = _hoverable.Hovered.Val ? _hoverColor : _originalColor;
        this.StartEaseCoroutine(ref _transitionCoroutine, _easeSettings, p =>
        {
            var color = Color.Lerp(from, to, p);
            foreach (var image in _images)
            {
                image.color = color;
            }
        });
    }
}
