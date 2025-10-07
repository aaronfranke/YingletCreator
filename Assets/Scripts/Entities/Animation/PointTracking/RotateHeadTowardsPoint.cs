using UnityEngine;

public class RotateHeadTowardsPoint : MonoBehaviour
{
    private IPointTrackingLocationProvider _locationProvider;

    private void Awake()
    {
        _locationProvider = this.GetComponentInParent<IPointTrackingLocationProvider>();
    }
    // TODO
}
