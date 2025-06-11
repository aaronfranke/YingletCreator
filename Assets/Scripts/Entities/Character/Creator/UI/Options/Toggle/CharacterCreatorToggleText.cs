using TMPro;
using UnityEngine;

namespace Character.Creator.UI
{
	public class CharacterCreatorToggleText : MonoBehaviour
	{
		private void Start()
		{
			var text = this.GetComponent<TMP_Text>();
			text.text = this.GetComponentInParent<ICharacterCreatorToggleReference>().DisplayName;
		}
	}
}