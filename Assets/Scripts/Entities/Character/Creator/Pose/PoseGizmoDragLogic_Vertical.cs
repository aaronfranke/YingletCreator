using UnityEngine;

internal sealed class PoseGizmoDragLogic_Vertical : MonoBehaviour, IPoseGizmoDragLogic
{
	public bool DragOnXZPlane => false;

	public void UpdateTransform(Transform target, Vector3 initialMousePos, Vector3 currentMousePos, Vector3 initialTargetPos, float initialTargetRot)
	{
		float verticalOffset = currentMousePos.y - initialMousePos.y;

		target.transform.position = initialTargetPos + Vector3.up * verticalOffset;
	}
}
