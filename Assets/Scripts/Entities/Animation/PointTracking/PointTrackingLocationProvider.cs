using Reactivity;
using UnityEngine;

public interface IPointTrackingLocationProvider
{
    bool Active { get; }
    Vector3 Position { get; }
}

public class PointTrackingLocationProvider : MonoBehaviour, IPointTrackingLocationProvider
{
    const float Distance = 1.4f;

    [SerializeField] GameObject _debugObject;
    Observable<bool> _active = new Observable<bool>(false);

    public bool Active => _active.Val;
    public Vector3 Position => _debugObject.transform.position;

    void Update()
    {
        _active.Val = Vector3.Distance(this.transform.position, _debugObject.transform.position) < Distance;
    }
}
