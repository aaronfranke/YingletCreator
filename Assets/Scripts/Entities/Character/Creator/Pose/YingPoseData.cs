using Reactivity;

/// <summary>
/// Pose data for an individual ying
/// For all the pose data, refer to IPoseData
/// </summary>
public interface IYingPoseData
{
	int MouthExpressionNum { get; set; }
	int EyeExpressionNum { get; set; }
}

internal sealed class YingPoseData : IYingPoseData
{
	// TODO: Pass in the observable data into the constructor and get the defaults
	Observable<int> _mouthExpressionNum = new();
	Observable<int> _eyeExpressionNum = new();

	public int MouthExpressionNum { get => _mouthExpressionNum.Val; set => _mouthExpressionNum.Val = value; }
	public int EyeExpressionNum { get => _eyeExpressionNum.Val; set => _eyeExpressionNum.Val = value; }
}
