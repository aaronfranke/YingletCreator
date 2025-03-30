using System.Collections.Generic;
using UnityEngine;

internal interface IApplyableCustomization
{
    void Apply();
}

/// <summary>
/// We want to apply all of these options in order, as some affect others
/// </summary>
public class ApplyCustomizationsAfterAnimate : MonoBehaviour
{
    private IEnumerable<IApplyableCustomization> _applyable;

    private void Awake()
    {
        _applyable = this.GetComponentsInChildren<IApplyableCustomization>();
    }

    // Do this every frame in late-update so it happens after the animator
    private void LateUpdate()
    {
        foreach (var a in _applyable)
        {
            a.Apply();
        }
    }
}
