using Character.Creator;
using Character.Data;
using Reactivity;

/// <summary>
/// Pose data for an individual ying
/// For all the pose data, refer to IPoseData
/// </summary>
public interface IYingPoseData
{
	string Name { get; }
	int EyeExpressionNum { get; set; }
	int MouthExpressionNum { get; set; }
	PoseId Pose { get; set; }
}

internal sealed class YingPoseData : IYingPoseData
{
	public YingPoseData(SerializableCustomizationData data)
	{
		// This isn't optimized; the visual ying itself is also doing this conversion
		// Also, this repeatedly happens as the ying is selected/deselected
		// But w/e we're all gonna die one day anyway so i'll eat the few ms to not care
		var observableData = new ObservableCustomizationData(data);
		Name = observableData.Name.Val;

		// Hard referencing them like this is a little cringe, but I also don't care about pose mode
		var eyeKey = ResourceLoader.Load<CharacterIntId>("1490a3cd97e05b34bb4efe5ed5513346");
		var mouthKey = ResourceLoader.Load<CharacterIntId>("6ccdbae82063b23438cc6d12818d35a0");

		_eyeExpressionNum.Val = observableData.NumberData.GetInt(eyeKey);
		_mouthExpressionNum.Val = observableData.NumberData.GetInt(mouthKey);
	}

	Observable<int> _eyeExpressionNum = new();
	Observable<int> _mouthExpressionNum = new();
	Observable<PoseId> _pose = new();

	public string Name { get; }
	public int EyeExpressionNum { get => _eyeExpressionNum.Val; set => _eyeExpressionNum.Val = value; }
	public int MouthExpressionNum { get => _mouthExpressionNum.Val; set => _mouthExpressionNum.Val = value; }
	public PoseId Pose { get => _pose.Val; set => _pose.Val = value; }
}
