using System.Collections;
using UnityEngine;

public class PlayAnimOnBooped : MonoBehaviour
{
    private Animator _animator;
    private IBoopManager _boopManager;
    private int _layerIndex;
    private int _stateNameHash;
    private Coroutine _removeWeightCoroutine;

    private void Awake()
    {

        _animator = this.GetComponent<Animator>();
        _boopManager = this.GetComponentInParent<IBoopManager>();
        _layerIndex = _animator.GetLayerIndex("Booped");
        _stateNameHash = Animator.StringToHash("Booped");

        _boopManager.OnBoop += OnBooped;
    }

    private void OnDestroy()
    {
        _boopManager.OnBoop -= OnBooped;
    }
    private void OnEnable()
    {
        _animator.SetLayerWeight(_layerIndex, 0f);
    }

    void OnBooped()
    {
        if (!this.isActiveAndEnabled) return;

        _animator.Play(_stateNameHash, _layerIndex, 0);
        _animator.SetLayerWeight(_layerIndex, 1f);
        this.StopAndStartCoroutine(ref _removeWeightCoroutine, RemoveWeightAfterTime());
    }

    IEnumerator RemoveWeightAfterTime()
    {
        yield return new WaitForSeconds(2.5f);
        _animator.SetLayerWeight(_layerIndex, 0f);
    }
}
