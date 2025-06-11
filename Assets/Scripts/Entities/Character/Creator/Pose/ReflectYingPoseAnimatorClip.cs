using Reactivity;
using UnityEngine;

public class ReflectYingPoseAnimatorClip : ReactiveBehaviour
{
	private IPoseYingDataRepository _dataRepo;
	private AnimatorOverrideController _overrideController;

	private void Start()
	{
		_dataRepo = this.GetComponentInParent<IPoseYingDataRepository>();
		var animator = this.GetComponent<Animator>();

		var originalController = animator.runtimeAnimatorController;
		_overrideController = new AnimatorOverrideController(originalController);
		animator.runtimeAnimatorController = _overrideController;

		AddReflector(Reflect);
	}

	private void Reflect()
	{
		var clip = _dataRepo.YingPoseData.Pose?.Clip;
		if (clip == null) return;

		var clips = _overrideController.animationClips;
		_overrideController[clips[0].name] = clip;
	}
}
