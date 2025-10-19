using Reactivity;
using UnityEngine;

public class ReflectPoseShadowEnabled : ReactiveBehaviour
{
	private IPoseYingDataRepository _dataRepo;
	private Renderer _renderer;

	void Start()
	{
		_dataRepo = this.GetComponentInParent<IPoseYingDataRepository>();
		_renderer = this.GetComponent<Renderer>();

		AddReflector(Reflect);
	}

	private void Reflect()
	{
		_renderer.enabled = _dataRepo.YingPoseData.Shadow;
	}
}
