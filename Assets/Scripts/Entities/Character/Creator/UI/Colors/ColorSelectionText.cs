using TMPro;
using UnityEngine;


namespace Character.Creator.UI
{
	public class ColorSelectionText : MonoBehaviour
	{
		private void Start()
		{
			var reference = this.GetComponentInParent<IColorSelectionReference>();
			this.GetComponent<TMP_Text>().text = reference.Id.name;
		}
	}
}