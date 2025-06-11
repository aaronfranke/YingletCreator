using Character.Data;
using UnityEngine;


namespace Character.Creator.UI
{
	public class CharacterCreatorTogglePoseGroup : MonoBehaviour
	{
		[SerializeField] PoseId[] _poseIds;
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
			foreach (var poseId in _poseIds)
			{
				var go = GameObject.Instantiate(_togglePrefab, this.transform);
				go.GetComponent<ICharacterCreatorTogglePoseReference>().PoseId = poseId;
			}
		}
	}
}