using UnityEngine;

/// <summary>
/// Currently a very basic implementation
/// Would want to be properly able to look at a Vector3 with each individual eye providing a different offset
/// Maybe also even a mechanism for cross eyed? (though that should maybe be its own thing)
/// </summary>
public class EyeLookAt : MonoBehaviour, IEyeOffsetProvider
{
	[SerializeField] Transform _headBone;
	[SerializeField] float _degreesToOffsetMult = .1f;
	private Transform _root;
	Vector3 _originalForward;

	void Awake()
	{
		_root = this.GetComponentInParent<YingletVisualsRoot>().transform;
		_originalForward = GetHeadForward();
	}

	private Vector3 GetHeadForward()
	{
		return _root.InverseTransformVector(_headBone.forward);
	}

	public Vector2 Offset
	{
		get
		{
			var angle = GetYRotation(_originalForward, GetHeadForward());
			if (angle > 180) angle -= 360;

			return new Vector2(angle * _degreesToOffsetMult, 0);
		}
	}
	public static float GetYRotation(Vector3 from, Vector3 to)
	{
		// Get the angle in radians between the two vectors on the XY plane
		float angle = Mathf.Atan2(to.x, to.z) - Mathf.Atan2(from.x, from.z);

		// Convert to degrees
		return Mathf.Rad2Deg * angle;
	}
}
