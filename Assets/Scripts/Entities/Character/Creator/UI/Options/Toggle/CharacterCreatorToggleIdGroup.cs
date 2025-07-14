using Character.Data;
using System.Linq;
using UnityEngine;


namespace Character.Creator.UI
{
	public class CharacterCreatorToggleIdGroup : MonoBehaviour
	{
		[SerializeField] CharacterToggleOrderGroup _group;
		[SerializeField] GameObject _togglePrefab;

		private void Awake()
		{
			foreach (Transform child in transform)
			{
				Destroy(child.gameObject);
			}
		}
		private void Start()
		{
			var allToggles = ResourceLoader.LoadAll<CharacterToggleId>().ToArray();
			var relevantToggles = allToggles
				.Where(toggle => toggle.Order.Group == _group)
				.OrderBy(toggle => toggle.Order.Index)
				.ToArray();
			foreach (var toggleId in relevantToggles)
			{
				var go = GameObject.Instantiate(_togglePrefab, this.transform);
				go.GetComponent<ICharacterCreatorToggleIdReference>().ToggleId = toggleId;
			}
		}
	}
}