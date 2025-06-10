using Character.Creator.UI;
using UnityEngine;

internal class PoseExpressionToggleAssigner : MonoBehaviour, IExpressionToggleAssigner
{
	[SerializeField] bool _eyesNotMouth;
	private IPageYingPoseData _poseData;

	void Awake()
	{
		_poseData = this.GetComponentInParent<IPageYingPoseData>();
	}

	public int Value
	{
		get
		{
			if (_poseData.Data == null) return 0;

			return _eyesNotMouth
				? _poseData.Data.EyeExpressionNum
				: _poseData.Data.MouthExpressionNum;

		}
		set
		{
			if (_poseData.Data == null) return;

			if (_eyesNotMouth) _poseData.Data.EyeExpressionNum = value;
			else _poseData.Data.MouthExpressionNum = value;
		}
	}
}
