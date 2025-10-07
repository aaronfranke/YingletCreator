using Character.Compositor;
using Reactivity;
using System.Collections;
using UnityEngine;

public class DisableSwayAnimationOnPointTracking : ReactiveBehaviour
{
    [SerializeField] AnimationCurve _toggleCurve;

    const float ToggleTime = 0.5f;

    private Animator _animator;
    private IPointTrackingLocationProvider _locationProvider;

    private int _layerIndex;
    private float _startWeight;
    private Coroutine _coroutine;

    private void Awake()
    {

        _animator = this.GetCompositedYingletComponent<Animator>();
        _layerIndex = _animator.GetLayerIndex("LookAround");
        _locationProvider = this.GetComponentInParent<IPointTrackingLocationProvider>();
        _startWeight = _animator.GetLayerWeight(_layerIndex);
    }

    private void Start()
    {
        AddReflector(ReflectActive);
    }

    private void OnEnable()
    {
        ReflectActive(); // In-case this got disabled mid weight change
    }

    private void ReflectActive()
    {
        this.StopAndStartCoroutine(ref _coroutine, ChangeWeightOverTime());
    }

    private IEnumerator ChangeWeightOverTime()
    {
        var from = _animator.GetLayerWeight(_layerIndex);
        var to = _locationProvider.Active ? 0f : _startWeight;
        for (float t = 0; t < ToggleTime; t += Time.deltaTime)
        {
            float p = t / ToggleTime;
            p = _toggleCurve.Evaluate(p);
            _animator.SetLayerWeight(_layerIndex, Mathf.Lerp(from, to, p));
            yield return null;
        }
        _animator.SetLayerWeight(_layerIndex, to);
    }
}
