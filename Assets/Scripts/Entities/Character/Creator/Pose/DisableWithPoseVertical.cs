using Reactivity;

public class DisableWithPoseVertical : ReactiveBehaviour
{
	private IPoseData _poseData;

	void Start()
	{
		_poseData = this.GetComponentInParent<IPoseData>();
		AddReflector(Reflect);
	}

	private void Reflect()
	{
		gameObject.SetActive(GetActive());
	}

	bool GetActive()
	{
		var data = _poseData.CurrentlyEditing;
		if (data == null) return false;

		if (!_poseData.Data.TryGetValue(data.Reference, out var _data)) return false;
		return _data.VerticalPositioning;
	}
}
