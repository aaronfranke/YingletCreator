using UnityEngine;

public class RotateHeadTowardsPoint : MonoBehaviour
{
    [SerializeField] Transform _rootHeadBone;
    [SerializeField] Transform _headCenter;

    [SerializeField] float _rotationSpeed = 8f;

    [SerializeField] float _maxAngle = 60f;

    [Tooltip("This corresponds to how much weight each bone down from the root should rotate by. This should not exceed 1 in either axis. No z-axis because we don't want to spin the head like that")]
    [SerializeField] Vector2[] _chainWeights;

    private IPointTrackingLocationProvider _locationProvider;
    private Transform[] _boneChain;

    private void Awake()
    {
        _locationProvider = GetComponentInParent<IPointTrackingLocationProvider>();

        // Collect bone chain upward from head
        _boneChain = new Transform[_chainWeights.Length];
        var current = _rootHeadBone;
        for (int i = 0; i < _chainWeights.Length; i++)
        {
            _boneChain[i] = current;
            current = current.parent;
        }
    }

    private void LateUpdate()
    {
        if (!_locationProvider.Active) return;

        RotateChainTowards(_locationProvider.Position);
    }

    private void RotateChainTowards(Vector3 targetPosition)
    {
        // Figure out how much we ideally want to rotate by
        var baseRotation = _rootHeadBone.rotation;
        var targetRotation = Quaternion.LookRotation(targetPosition - _headCenter.position, Vector3.up);
        var rotationAmount = Quaternion.Inverse(baseRotation) * targetRotation;

        // We don't want characters turning super far, so clamp down on the possible angle
        rotationAmount.ToAngleAxis(out float angle, out Vector3 axis);
        if (angle > 180f) angle -= 360f; // normalize angle range
        float clampedAngle = Mathf.Clamp(angle, -_maxAngle, _maxAngle);
        var clampedRotationAmount = Quaternion.AngleAxis(clampedAngle, axis);

        var eulerRotationAmount = clampedRotationAmount.eulerAngles.DirectionalizeEulerAngles();

        for (int i = 0; i < _boneChain.Length; i++)
        {
            var bone = _boneChain[i];
            var weight = _chainWeights[i];
            var weightedRot = Quaternion.Euler(eulerRotationAmount.x * weight.x, eulerRotationAmount.y * weight.y, 0);
            bone.localRotation = bone.localRotation * weightedRot;
        }
    }
}