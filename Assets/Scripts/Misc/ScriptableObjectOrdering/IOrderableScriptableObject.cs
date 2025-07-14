using UnityEngine;

public interface IOrderableScriptableObject<TGroup> where TGroup : ScriptableObject
{
	public IOrderData<TGroup> Order { get; }
}

public interface IOrderData<TGroup> where TGroup : ScriptableObject
{
	public TGroup Group { get; }

	public int Index { get; }
}