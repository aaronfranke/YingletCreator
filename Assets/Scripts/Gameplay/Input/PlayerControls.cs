using System.Collections;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
	[SerializeField] MovableEntity _debugMovableEntity;

	void Start()
	{
		StartCoroutine(ProcessPlayerInput());
	}

	IEnumerator ProcessPlayerInput()
	{
		while (true)
		{
			if (Input.GetMouseButton(1))
			{

				_debugMovableEntity.Move(GetMouseWorldPosition());
			}
			yield return null;
		}
	}


	Vector3 GetMouseWorldPosition()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		Plane xzPlane = new Plane(Vector3.up, Vector3.zero); // Plane at y = 0

		if (xzPlane.Raycast(ray, out float distance))
		{
			return ray.GetPoint(distance);
		}

		return Vector3.zero; // Default return if something goes wrong
	}
}
