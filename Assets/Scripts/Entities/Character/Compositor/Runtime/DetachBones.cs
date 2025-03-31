using UnityEngine;
/// <summary>
/// Detaches bones, so that operations won't affect the positions/scales of children
/// These get reattached on the next frame by CopyRigAfterAnimate
/// </summary>
public class DetachBones : MonoBehaviour, IApplyableCustomization
{
    [SerializeField] Transform[] _bonesToDetach;

    public void Apply()
    {
        foreach (var bone in _bonesToDetach)
        {
            bone.SetParent(null, true);
        }
    }

}