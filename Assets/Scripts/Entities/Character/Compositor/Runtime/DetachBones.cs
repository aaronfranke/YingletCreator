using UnityEngine;
/// <summary>
/// Detaches bones, so that operations won't affect the positions/scales of children
/// These get reattached on the next frame by CopyRigAfterAnimate
/// </summary>
public class DetachBones : MonoBehaviour, IApplyableCustomization
{
    [SerializeField] Transform[] _bonesToDetach;
    private Transform _root;

    void Awake()
    {
        _root = this.GetComponentInParent<YingletVisualsRoot>().transform;
    }

    public void Apply()
    {
        foreach (var bone in _bonesToDetach)
        {
            bone.SetParent(_root, true);
        }
    }

}