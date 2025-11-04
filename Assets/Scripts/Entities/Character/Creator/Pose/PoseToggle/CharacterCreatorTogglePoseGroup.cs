using Character.Data;
using System.Linq;
using UnityEngine;



namespace Character.Creator.UI
{
	public class CharacterCreatorTogglePoseGroup : MonoBehaviour
	{
		private ICompositeResourceLoader _resourceLoader;
		[SerializeField] AssetReferenceT<PoseOrderGroup> _groupReference;
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
			var allPoses = _resourceLoader.LoadAllPoseIds().ToArray();
			var relevantPoses = allPoses
				.Where(pose => pose.Order.Group == _groupReference.LoadSync())
				.OrderBy(pose => pose.Order.Index)
				.ToArray();
			foreach (var poseId in relevantPoses)
			{
				var go = GameObject.Instantiate(_togglePrefab, this.transform);
				go.GetComponent<ICharacterCreatorTogglePoseReference>().PoseId = poseId;
			}
		}
	}
}