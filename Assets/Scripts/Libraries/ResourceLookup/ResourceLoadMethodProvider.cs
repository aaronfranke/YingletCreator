using UnityEngine;
public enum CompositeResourceLoadMethod
{
	EditorAssetLookup,
	SerializedTableLookup
}

public interface IResourceLoadMethodProvider
{
	CompositeResourceLoadMethod LoadMethod { get; }
}
public class ResourceLoadMethodProvider : MonoBehaviour, IResourceLoadMethodProvider
{
	[SerializeField] CompositeResourceLoadMethod _loadMethod = CompositeResourceLoadMethod.EditorAssetLookup;

	public CompositeResourceLoadMethod LoadMethod
	{
		get
		{
			var desiredLoadMethod = _loadMethod;
			var actualLoadMethod = CompositeResourceLoadMethod.SerializedTableLookup;
#if UNITY_EDITOR
			if (desiredLoadMethod == CompositeResourceLoadMethod.EditorAssetLookup)
			{
				actualLoadMethod = CompositeResourceLoadMethod.EditorAssetLookup;
			}
#endif
			return actualLoadMethod;
		}
	}
}