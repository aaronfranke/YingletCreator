using UnityEngine;

public class PlayAnimOnBooped : MonoBehaviour
{
    private Animator _animator;
    private IBoopManager _boopManager;
    private int _layerIndex;
    private int _stateNameHash;

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

    void OnBooped()
    {
        _animator.Play(_stateNameHash, _layerIndex, 0);
        _animator.SetLayerWeight(_layerIndex, 1f);
    }
}
