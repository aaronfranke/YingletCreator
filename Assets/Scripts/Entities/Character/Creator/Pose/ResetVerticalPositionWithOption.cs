using Reactivity;
using UnityEngine;

public class ResetVerticalPositionWithOption : ReactiveBehaviour
{
	private IPoseYingDataRepository _poseData;

	void Start()
	{
		_poseData = this.GetComponentInParent<IPoseYingDataRepository>();
		AddReflector(Reflect);
	}

	void Reflect()
	{
		if (_poseData.YingPoseData.VerticalPositioning == false)
		{
			this.transform.position = new Vector3(this.transform.position.x, 0f, this.transform.position.z);
		}
	}
}
