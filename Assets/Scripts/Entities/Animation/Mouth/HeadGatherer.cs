using UnityEngine;

public interface IHeadGatherer
{
	Material HeadMaterial { get; }
}

public class HeadGatherer : MonoBehaviour, IHeadGatherer
{
	[SerializeField] Transform _rigRoot;
	Material _cached;

	const string HEAD_NAME = "Head";

	public Material HeadMaterial
	{
		get
		{
			if (_cached != null) return _cached;

			_cached = GetHeadMaterial(HEAD_NAME);


			return _cached;
		}
	}

	Material GetHeadMaterial(string name)
	{
		var headTransform = TransformUtils.FindChildRecursive(_rigRoot, name);
		if (headTransform == null)
		{
			Debug.LogError($"Failed to find head {name}");
			return null;
		}
		return headTransform.GetComponent<SkinnedMeshRenderer>().sharedMaterial;
	}
}
