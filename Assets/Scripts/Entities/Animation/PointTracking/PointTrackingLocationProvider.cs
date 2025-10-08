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
    [SerializeField] float OffsetMouseFromPlane = .2f;

    Observable<bool> _active = new Observable<bool>(false);

    public bool Active => _active.Val;
    public Vector3 Position { get; private set; }

    void Update()
    {
        var camForward = Camera.main.transform.forward;
        var planeCenter = _headCenter.position - camForward * OffsetMouseFromPlane;
        Plane cursorPlane = new Plane(camForward, planeCenter);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (!cursorPlane.Raycast(ray, out float enter))
        {
            _active.Val = false;
            return;
        }
        var hitPoint = ray.GetPoint(enter);
        if (Vector3.Distance(hitPoint, planeCenter) > MaxDistance)
        {
            _active.Val = false;
            return;
        }

        Position = hitPoint;
        _active.Val = true;
    }
}
