using Reactivity;
using UnityEngine;

[RequireComponent(typeof(HoverableDetector))]
public class ScaleOnHover : ReactiveBehaviour
{
    [SerializeField] float _hoverScale;
    [SerializeField] SharedEaseSettings _easeSettings;
    private IHoverableDetector _hoverable;
    private Coroutine _transitionCoroutine;

    private void Awake()
    {
        _hoverable = this.GetComponent<IHoverableDetector>();
    }

    void Start()
    {
        AddReflector(Reflect);
    }

    private void Reflect()
    {
        Vector3 from = this.transform.localScale;
        Vector3 to = _hoverable.Hovered.Val ? Vector3.one * _hoverScale : Vector3.one;
        this.StartEaseCoroutine(ref _transitionCoroutine, _easeSettings, p => this.transform.localScale = Vector3.LerpUnclamped(from, to, p));
    }
}
