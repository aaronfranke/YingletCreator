using Reactivity;

public class PoseYingEditingGizmos : ReactiveBehaviour
{
	private IPoseData _poseData;

	private void Awake()
	{
		_poseData = this.GetComponentInParent<IPoseData>();
		AddReflector(ReflectSelected);
	}

	private void ReflectSelected()
	{
		var currentlyEditing = _poseData.CurrentlyEditing;
		this.gameObject.SetActive(currentlyEditing != null);
	}

	void LateUpdate()
	{
		var currentlyEditing = _poseData.CurrentlyEditing;
		if (currentlyEditing == null) return;

		this.transform.position = currentlyEditing.GameObject.transform.position;
		this.transform.localScale = currentlyEditing.GameObject.transform.localScale;
	}
}
