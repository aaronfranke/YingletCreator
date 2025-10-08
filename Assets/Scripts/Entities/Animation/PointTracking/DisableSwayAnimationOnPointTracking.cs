using Character.Compositor;
using Reactivity;
using UnityEngine;

public class DisableSwayAnimationOnPointTracking : ReactiveBehaviour
{
    private Animator _animator;
    private IPointTrackingWeightProvider _weightProvider;

    private int _layerIndex;
    private float _startWeight;
    private Coroutine _coroutine;

    private void Start()
    {
        _weightProvider = this.GetComponentInParent<IPointTrackingWeightProvider>();
        _animator = this.GetCompositedYingletComponent<Animator>();
        _layerIndex = _animator.GetLayerIndex("LookAround");
        _startWeight = _animator.GetLayerWeight(_layerIndex);

        AddReflector(Reflect);
    }

    private void Reflect()
    {
        var val = Mathf.Lerp(_startWeight, 0, _weightProvider.Weight);
        _animator.SetLayerWeight(_layerIndex, val);
    }
}
