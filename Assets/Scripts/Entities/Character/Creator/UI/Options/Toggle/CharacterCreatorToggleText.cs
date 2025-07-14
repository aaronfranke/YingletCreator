using TMPro;
using UnityEngine;

namespace Character.Creator.UI
{
	public class CharacterCreatorToggleText : MonoBehaviour
	{
		private void Start()
		{
			var text = this.GetComponent<TMP_Text>();
			var reference = this.GetComponentInParent<ICharacterCreatorToggleReference>();
			text.text = reference.DisplayName;

			// Commented code to make it easier to set real names
			//var actualReference = reference as CharacterCreatorToggleIdReference;
			//var realName = actualReference.ToggleId.name;
			//text.text = realName.Length >= 16 ? realName.Substring(realName.Length - 16) : realName;
		}
	}
}