using UnityEngine;
using UnityEngine.UI;

namespace Character.Creator.UI
{
	public class CharacterCreatorToggleSprite : MonoBehaviour
	{
		public void Start()
		{
			var image = this.GetComponent<Image>();
			image.sprite = this.GetComponentInParent<ICharacterCreatorToggleReference>().Sprite;
		}
	}
}