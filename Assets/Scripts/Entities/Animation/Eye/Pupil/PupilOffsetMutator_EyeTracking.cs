using UnityEngine;

public class PupilOffsetMutator_EyeTracking : MonoBehaviour, IPupilOffsetMutator
{
    [SerializeField] Transform _leftEye;
    [SerializeField] Transform _rightEye;

    [SerializeField] AnimationCurve _xAngleToPupilOffset;
    [SerializeField] AnimationCurve _yAngleToPupilOffset;


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


        var rightEyeAngles = GetEyeLookAngles(_rightEye, _locationProvider.Position);
        var leftEyeAngles = GetEyeLookAngles(_leftEye, _locationProvider.Position);

        var rightOffset = _xAngleToPupilOffset.Evaluate(rightEyeAngles.x);
        var leftOffset = _xAngleToPupilOffset.Evaluate(-leftEyeAngles.x);
        var averageYAngle = (rightEyeAngles.y + leftEyeAngles.y) / 2f;
        var yOffset = -_yAngleToPupilOffset.Evaluate(averageYAngle); // Negate it because up is negative y in UV space

        // Eventually do weight lerping here from the original
        var lookOffsets = new PupilOffsets(yOffset, leftOffset, rightOffset);
        return PupilOffsets.Lerp(input, lookOffsets, _weightProvider.Weight);
    }

    public static Vector2 GetEyeLookAngles(Transform eye, Vector3 targetPosition)
    {
        var toTargetWorld = targetPosition - eye.position; // world space direction

        var toTargetLocal = eye.InverseTransformDirection(toTargetWorld.normalized); // local space direction

        // Compute local-axis rotation angles
        float xAngle = Mathf.Atan2(toTargetLocal.z, toTargetLocal.y) * Mathf.Rad2Deg;   // Up/down
        float zAngle = -Mathf.Atan2(toTargetLocal.x, toTargetLocal.y) * Mathf.Rad2Deg;  // Left/right

        // new Vector3(xAngle, 0f, zAngle) would be the actual euler angles needed to rotate the eye
        // but let's change the axis and signs to better match what we need

        return new Vector2(zAngle, -xAngle);
    }
}
