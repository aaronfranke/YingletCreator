using Character.Data;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;


namespace Character.Creator.UI
{
	public class CharacterCreatorToggleIdGroup : MonoBehaviour
	{
		private ICompositeResourceLoader _resourceLoader;

		[SerializeField] AssetReferenceT<CharacterToggleOrderGroup> _groupReference;
		[SerializeField] GameObject _togglePrefab;

		private void Awake()
		{
			_resourceLoader = Singletons.GetSingleton<ICompositeResourceLoader>();
			foreach (Transform child in transform)
			{
				Destroy(child.gameObject);
			}
		}
		private void Start()
		{
			var group = _groupReference.LoadSync();
			var allToggles = _resourceLoader.LoadAllToggleIds().ToArray();
			var relevantToggles = allToggles
				.Where(toggle => toggle.Order.Group == group)
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