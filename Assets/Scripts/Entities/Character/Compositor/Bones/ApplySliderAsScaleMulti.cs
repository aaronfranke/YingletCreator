using UnityEngine;

public class ApplySliderAsScaleMulti : ApplySliderAsScaleBase, IApplyableCustomization
{
	[SerializeField] Transform[] _targets;

	public void Apply()
	{
		var size = GetSize();
		foreach (var target in _targets)
		{
			ApplyToTarget(target, size);
		}
	}
}
