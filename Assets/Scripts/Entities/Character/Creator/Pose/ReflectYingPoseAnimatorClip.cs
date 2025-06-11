using Reactivity;
using System.Collections.Generic;
using UnityEngine;

public class ReflectYingPoseAnimatorClip : ReactiveBehaviour
{
	private IPoseYingDataRepository _dataRepo;
	private Animator _animator;
	private AnimatorOverrideController _overrideController;
	private AnimationClip _originalClip;

	private void Start()
	{
		_dataRepo = this.GetComponentInParent<IPoseYingDataRepository>();
		_animator = this.GetComponent<Animator>();

		var originalController = _animator.runtimeAnimatorController;
		_overrideController = new AnimatorOverrideController(originalController);
		_animator.runtimeAnimatorController = _overrideController;
		_originalClip = _overrideController.animationClips[0];

		AddReflector(Reflect);
	}

	private void Reflect()
	{
		var clip = _dataRepo.YingPoseData.Pose?.Clip;
		if (clip == null) return;

		_overrideController.ApplyOverrides(new List<KeyValuePair<AnimationClip, AnimationClip>>() { new(_originalClip, clip) });

	}
}
