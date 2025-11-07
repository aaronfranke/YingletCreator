[System.Serializable]
public struct ResourcePair
{
	public string Guid;
	public UnityEngine.Object Object;
}
[System.Serializable]
public class ResourceLookupTable
{
	public ResourcePair[] Resources;
}
