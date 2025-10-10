using Reactivity;
using UnityEngine;

public interface IPointTrackingLocationProvider
{
    IReadOnlyObservable<bool> Active { get; }
    Vector3 Position { get; }
    Vector3 ForwardDir { get; }
}

public class PointTrackingLocationProvider : MonoBehaviour, IPointTrackingLocationProvider
{
    [SerializeField] Transform _headCenter;
    [SerializeField] Transform _forwardProvider;

    [SerializeField] float MaxDistance = 1.4f;
    [SerializeField] float OffsetMouseFromPlane = .2f;
    [SerializeField] float MaxDotProduct = 0;

    Observable<bool> _active = new Observable<bool>(false);

    public IReadOnlyObservable<bool> Active => _active;
    public Vector3 Position { get; private set; }

    public Vector3 ForwardDir => -_forwardProvider.forward;

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

        var direction = (hitPoint - _headCenter.transform.position).normalized;
        if (Vector3.Dot(ForwardDir, direction) < MaxDotProduct)
        {
            _active.Val = false;
            return;
        }

        Position = hitPoint;
        _active.Val = true;
    }
}
