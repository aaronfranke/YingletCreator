using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class YingletAnimations : MonoBehaviour
{
    [SerializeField] AnimationClip _tPoseClip;
    [SerializeField] AnimationClip[] _additiveClips;

    private Animator _animator;

    private void Awake()
    {
        _animator = this.GetComponent<Animator>();
        foreach (var clip in _additiveClips)
        {
            AnimationUtility.SetAdditiveReferencePose(clip, _tPoseClip, 0);
        }
    }
}
