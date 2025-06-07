using Character.Creator.UI;
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

		// TODO: Add in transform and fix this
		//this.transform.position = hovered.transform.position;
		//this.transform.localScale = hovered.transform.localScale;
	}
}
