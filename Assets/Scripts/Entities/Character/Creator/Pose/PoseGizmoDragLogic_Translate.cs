using UnityEngine;

internal interface IPoseGizmoDragLogic
{
	void UpdateTransform(Transform target, Vector3 initialMousePos, Vector3 currentMousePos, Vector3 initialTargetPos, float initialTargetRot);
}

internal sealed class PoseGizmoDragLogic_Translate : MonoBehaviour, IPoseGizmoDragLogic
{
	public void UpdateTransform(Transform target, Vector3 initialMousePos, Vector3 currentMousePos, Vector3 initialTargetPos, float initialTargetRot)
	{
		Vector3 offset = initialTargetPos - initialMousePos;

		target.transform.position = currentMousePos + offset;
	}
}
