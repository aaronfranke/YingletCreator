using Reactivity;
using UnityEngine;

public interface IPoseCameraData
{
	float FieldOfView { get; set; }
	float Speed { get; set; }
}
internal class PoseCameraData : MonoBehaviour, IPoseCameraData
{
	Observable<float> _fieldOfView = new();
	Observable<float> _speed = new(1f);

	private void Awake()
	{
		_fieldOfView.Val = Camera.main.fieldOfView;
	}

	public float FieldOfView { get => _fieldOfView.Val; set => _fieldOfView.Val = value; }
	public float Speed { get => _speed.Val; set => _speed.Val = value; }
}
