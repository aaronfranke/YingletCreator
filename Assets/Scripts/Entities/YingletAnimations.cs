using System;
using CharacterCompositor;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class YingletAnimations : MonoBehaviour
{
    private ICompositedYinglet _compositedYinglet;
    private Animator _animator;

    private void Awake()
    {
        _compositedYinglet = this.GetComponent<ICompositedYinglet>();
        _animator = this.GetComponent<Animator>();

        _compositedYinglet.OnSkinnedMeshRenderersRegenerated += OnSkinnedMeshRenderersRegenerated;
    }
    private void OnDestroy()
    {
        _compositedYinglet.OnSkinnedMeshRenderersRegenerated -= OnSkinnedMeshRenderersRegenerated;
    }

    private void OnSkinnedMeshRenderersRegenerated()
    {
        // The animator won't play nicely with the new SkinnedMeshRenderers unless we give it a lil' kick
        _animator.Rebind();
        _animator.Update(0);
    }
}
