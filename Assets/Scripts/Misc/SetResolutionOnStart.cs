using UnityEngine;

public class SetResolutionOnStart : MonoBehaviour
{

	void Start()
	{
		Screen.SetResolution(1920, 1080, FullScreenMode.Windowed);
	}
}
