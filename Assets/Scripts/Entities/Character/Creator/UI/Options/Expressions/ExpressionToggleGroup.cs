using UnityEngine;

namespace Character.Creator.UI
{
	public class ExpressionToggleGroup : MonoBehaviour
	{
		[SerializeField] Sprite[] _sprites;
		[SerializeField] GameObject _prefab;

		private void Awake()
		{
			foreach (Transform child in transform)
			{
				Destroy(child.gameObject);
			}
		}
		void Start()
		{
			for (int i = 0; i < _sprites.Length; i++)
			{
				var sprite = _sprites[i];
				var go = GameObject.Instantiate(_prefab, this.transform);
				go.GetComponent<IExpressionToggle>().Setup(i, sprite);
			}
		}
	}
}