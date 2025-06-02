using UnityEngine;

public class SpinYingletOnMouse : MonoBehaviour
{
	[SerializeField] float _spinSensitivity = 10f;

	void Update()
	{
		if (Input.GetMouseButton(1))
		{
			float spinAmount = Input.GetAxisRaw("Mouse X") * _spinSensitivity;
			this.transform.rotation *= Quaternion.Euler(0, spinAmount, 0);
		}

	}
}
