using Reactivity;

public class PoseYingEditingGizmos : ReactiveBehaviour
{
	private IPoseData _poseData;
	private ISmartDisabler _smartDisabler;

	private void Awake()
	{
		_poseData = this.GetComponentInParent<IPoseData>();
		_smartDisabler = this.GetComponent<ISmartDisabler>();
		AddReflector(ReflectSelected);
	}

	private void ReflectSelected()
	{
		var currentlyEditing = _poseData.CurrentlyEditing;
		_smartDisabler.SetActive(currentlyEditing != null, this);
	}

	void LateUpdate()
	{
		var currentlyEditing = _poseData.CurrentlyEditing;
		if (currentlyEditing == null) return;

		this.transform.position = currentlyEditing.GameObject.transform.position;
		this.transform.localScale = currentlyEditing.GameObject.transform.localScale;
	}
}
