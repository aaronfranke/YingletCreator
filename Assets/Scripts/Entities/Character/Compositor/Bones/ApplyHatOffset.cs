using Character.Creator;
using Character.Data;
using Reactivity;
using System.Linq;
using UnityEngine;


public class ApplyHatOffset : ReactiveBehaviour, IApplyableCustomization
{
	[SerializeField] Transform _target;

	ICustomizationSelectedDataRepository _dataRepository;
	private Computed<float> _computeOffset;

	private void Awake()
	{
		_dataRepository = GetComponentInParent<ICustomizationSelectedDataRepository>();
		_computeOffset = CreateComputed(ComputeOffset);
	}

	private float ComputeOffset()
	{
		var toggles = _dataRepository.CustomizationData.ToggleData.Toggles.ToArray();
		foreach (var toggle in toggles)
		{
			foreach (var component in toggle.Components)
			{
				if (component is IOffsetHatBone offsetHatBone)
				{
					return offsetHatBone.Amount;
				}
			}
		}
		return 0;
	}

	public void Apply()
	{
		var offset = _computeOffset.Val;
		Debug.Log("Applying offset of " + offset + " to hat bone " + _target.name, this);
		_target.transform.localPosition += Vector3.up * offset;

	}

}
