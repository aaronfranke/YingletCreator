using UnityEngine;

public class RotateVerticalGizmoTowardsCamera : MonoBehaviour
{

	// Update is called once per frame
	void LateUpdate()
	{
		var camera = Camera.main.transform;
		Vector3 direction = camera.position - this.transform.position;

		// Ignore vertical difference
		direction.y = 0f;
		if (direction.sqrMagnitude > 0.001f)
		{
			this.transform.rotation = Quaternion.LookRotation(-direction, Vector3.up);
		}
	}
}
