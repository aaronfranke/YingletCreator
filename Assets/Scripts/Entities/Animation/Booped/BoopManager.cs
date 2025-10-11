using Reactivity;
using System;
using UnityEngine;

public interface IBoopManager
{
    event Action OnBoop;
}
public class BoopManager : ReactiveBehaviour, IBoopManager
{
    public event Action OnBoop;

    private IColliderHoverManager _colliderHoverManager;
    private Computed<bool> _hoveringBoopHitbox;


    private void Start()
    {
        _colliderHoverManager = Singletons.GetSingleton<IColliderHoverManager>();

        _hoveringBoopHitbox = CreateComputed(ComputeHoveringBoopHitbox);
    }

    private bool ComputeHoveringBoopHitbox()
    {
        var hovered = _colliderHoverManager.CurrentlyHovered;
        if (hovered == null) return false;
        return hovered.gameObject.GetComponent<BoopHitbox>() != null;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && _hoveringBoopHitbox.Val)
        {
            OnBoop?.Invoke();
        }
    }
}
