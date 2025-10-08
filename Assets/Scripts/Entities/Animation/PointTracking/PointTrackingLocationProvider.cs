using Reactivity;
using UnityEngine;

public interface IPointTrackingLocationProvider
{
    bool Active { get; }
    Vector3 Position { get; }
}

public class PointTrackingLocationProvider : MonoBehaviour, IPointTrackingLocationProvider
{
    [SerializeField] Transform _headCenter;

    [SerializeField] float MaxDistance = 1.4f;

    Observable<bool> _active = new Observable<bool>(false);

    public bool Active => _active.Val;
    public Vector3 Position { get; private set; }

    void Update()
    {
        Plane cursorPlane = new Plane(Camera.main.transform.forward, _headCenter.position);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (!cursorPlane.Raycast(ray, out float enter))
        {
            _active.Val = false;
            return;
        }
        var hitPoint = ray.GetPoint(enter);
        if (Vector3.Distance(hitPoint, _headCenter.transform.position) > MaxDistance)
        {
            _active.Val = false;
            return;
        }

        Position = hitPoint;
        _active.Val = true;
    }
}
