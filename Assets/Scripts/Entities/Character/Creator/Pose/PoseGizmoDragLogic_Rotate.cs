using UnityEngine;

internal sealed class PoseGizmoDragLogic_Rotate : MonoBehaviour, IPoseGizmoDragLogic
{
	public void UpdateTransform(Transform target, Vector3 initialMousePos, Vector3 currentMousePos, Vector3 initialTargetPos, float initialTargetRot)
	{
		Vector2 initial = new Vector2(initialMousePos.x, initialMousePos.z);
		Vector2 current = new Vector2(currentMousePos.x, currentMousePos.z);
		Vector2 pivot = new Vector2(initialTargetPos.x, initialTargetPos.z);


		Vector2 initialDir = initial - pivot;
		Vector2 currentDir = current - pivot;

		// Normalize the direction vectors
		initialDir.Normalize();
		currentDir.Normalize();

		// Compute the angle in degrees between the two vectors
		float angle = Vector2.SignedAngle(initialDir, currentDir);

		target.transform.localRotation = Quaternion.Euler(0, initialTargetRot - angle, 0);
		this.transform.localRotation = Quaternion.Euler(0, -angle, 0);
	}
}
