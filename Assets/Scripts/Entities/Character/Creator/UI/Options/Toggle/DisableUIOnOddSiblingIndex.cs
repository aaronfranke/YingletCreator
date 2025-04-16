using UnityEngine;
using UnityEngine.UI;

public class DisableUIOnOddSiblingIndex : MonoBehaviour
{
	[SerializeField] Graphic _target;
	void Start()
	{
		_target.enabled = this.transform.GetSiblingIndex() % 2 == 0;
	}
}
