using Character.Creator;
using Character.Creator.UI;
using Reactivity;
using UnityEngine;

public class ReflectPoseYingObjects : ReactiveBehaviour
{
	[SerializeField] GameObject _yingletPrefab;
	private IPoseData _poseData;
	EnumerableDictReflector<CachedYingletReference, GameObject> _enumerableReflector;

	private void Start()
	{
		_poseData = this.GetComponentInParent<IPoseData>();
		_enumerableReflector = new EnumerableDictReflector<CachedYingletReference, GameObject>(CreateYingObject, DeleteYingObject);
		AddReflector(Reflect);
	}


	private void Reflect()
	{
		_enumerableReflector.Enumerate(_poseData.Data.Keys);
	}

	private GameObject CreateYingObject(CachedYingletReference reference)
	{
		using (_yingletPrefab.TemporarilyDisable())
		{
			var yingObject = Instantiate(_yingletPrefab, transform);
			yingObject.GetComponent<IPoseYingDataRepository>().Setup(reference);
			yingObject.SetActive(true);
			return yingObject;
		}
	}
	private void DeleteYingObject(GameObject gameObject)
	{
		GameObject.Destroy(gameObject);
	}

}
