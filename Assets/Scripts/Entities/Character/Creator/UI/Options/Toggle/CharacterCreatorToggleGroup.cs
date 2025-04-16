using Character.Data;
using UnityEngine;


namespace Character.Creator.UI
{
	public class CharacterCreatorToggleGroup : MonoBehaviour
	{
		[SerializeField] CharacterToggleId[] _toggleIds;
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
			foreach (var toggleId in _toggleIds)
			{
				var go = GameObject.Instantiate(_togglePrefab, this.transform);
				go.GetComponent<ICharacterCreatorToggleReference>().ToggleId = toggleId;
			}
		}
	}
}