using UnityEngine;

public class PupilOffsetMutator_EyeTracking : MonoBehaviour, IPupilOffsetMutator
{
    [SerializeField] Transform _leftEye;
    [SerializeField] Transform _rightEye;


    private IPointTrackingLocationProvider _locationProvider;
    private IPointTrackingWeightProvider _weightProvider;

    void Awake()
    {
        _locationProvider = this.GetComponentInParent<IPointTrackingLocationProvider>();
        _weightProvider = this.GetComponentInParent<IPointTrackingWeightProvider>();
    }

    public PupilOffsets Mutate(PupilOffsets input)
    {
        if (_weightProvider.Weight < 0.01f) return input; // No change if not tracking

        // TODO continue from here
        var rotation = Quaternion.LookRotation(_leftEye.position - transform.position, _leftEye.forward);
        Debug.Log(rotation.eulerAngles);

        return input;

    }
}
