using UnityEngine;

public interface IMovableEntity
{
	public void Move(Vector3 position);
}

public class MovableEntity : MonoBehaviour, IMovableEntity
{
	public void Move(Vector3 position)
	{
		this.transform.position = position;
	}
}
