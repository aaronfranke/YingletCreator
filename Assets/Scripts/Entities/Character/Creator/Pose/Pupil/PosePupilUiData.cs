using UnityEngine;

public interface IPosePupilUiData
{
	Vector2 PupilPosition { get; set; }
}
public class PosePupilUiData : MonoBehaviour, IPosePupilUiData
{
	[SerializeField] bool _isLeft;
	IPageYingPoseData _poseData;
	private void Awake()
	{
		_poseData = this.GetComponentInParent<IPageYingPoseData>();
	}

	public Vector2 PupilPosition
	{
		get
		{
			var data = _poseData.Data;
			if (data == null) return Vector2.zero;
			var pupilData = data.PupilData.Val;
			return _isLeft
				? pupilData.GetLeftEyeOffsets()
				: pupilData.GetRightEyeOffsets();
		}
		set
		{
			if (_poseData.Data == null) return;

			var lastVal = _poseData.Data.PupilData.Val;
			if (_isLeft)
			{
				_poseData.Data.PupilData.Val = new YingPosePupilData(value.y, value.x, lastVal.XRightOffset);
			}
			else
			{
				_poseData.Data.PupilData.Val = new YingPosePupilData(value.y, lastVal.XLeftOffset, value.x);
			}
		}
	}
}
