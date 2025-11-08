[System.Serializable]
public struct ResourcePair
{
	public string Guid;
	public UnityEngine.Object Object;

	public ResourcePair(string guid, UnityEngine.Object obj)
	{
		this.Guid = guid;
		this.Object = obj;
	}
}
[System.Serializable]
public class ResourceLookupTable
{
	public ResourcePair[] Resources;
}
