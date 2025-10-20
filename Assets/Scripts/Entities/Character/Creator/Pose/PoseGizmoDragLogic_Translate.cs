using UnityEngine;

internal interface IPoseGizmoDragLogic
{
	void UpdateTransform(Transform target, Vector3 initialMousePos, Vector3 currentMousePos, Vector3 initialTargetPos, float initialTargetRot);
	bool DragOnXZPlane { get; }
}

internal sealed class PoseGizmoDragLogic_Translate : MonoBehaviour, IPoseGizmoDragLogic
{
	public bool DragOnXZPlane => true;

	public void UpdateTransform(Transform target, Vector3 initialMousePos, Vector3 currentMousePos, Vector3 initialTargetPos, float initialTargetRot)
	{
		Vector3 offset = initialTargetPos - initialMousePos;

		target.transform.position = currentMousePos + offset;
	}
}
