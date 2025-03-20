using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class YingletAnimations : MonoBehaviour
{
    private Animator _animator;

    private void Awake()
    {
        _animator = this.GetComponent<Animator>();
    }
}
