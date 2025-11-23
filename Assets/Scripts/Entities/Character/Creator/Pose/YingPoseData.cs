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

	bool Mirror { get; set; }
	bool Shadow { get; set; }
	bool VerticalPositioning { get; set; }
	PoseId Pose { get; set; }
	PupilOffsets PupilData { get; set; }
}

internal sealed class YingPoseData : IYingPoseData
{
	public YingPoseData(SerializableCustomizationData data, ICompositeResourceLoader resourceLoader)
	{
		// This isn't optimized; the visual ying itself is also doing this conversion
		// Also, this repeatedly happens as the ying is selected/deselected
		// But w/e we're all gonna die one day anyway so i'll eat the few ms to not care
		var observableData = new ObservableCustomizationData(data, resourceLoader);
		Name = observableData.Name.Val;

		// Hard referencing them like this is a little cringe, but I also don't care about pose mode
		var eyeKey = resourceLoader.Load<CharacterIntId>("1490a3cd97e05b34bb4efe5ed5513346");
		var mouthKey = resourceLoader.Load<CharacterIntId>("6ccdbae82063b23438cc6d12818d35a0");
		var poseKey = resourceLoader.Load<PoseId>("5c1ee6caa6f3f6c439595b0f1c5a27d4");

		_eyeExpressionNum.Val = observableData.NumberData.GetInt(eyeKey);
		_mouthExpressionNum.Val = observableData.NumberData.GetInt(mouthKey);
		_pose.Val = poseKey;

		_pupilData = new Observable<PupilOffsets>(PupilOffsetMutator_Default.InherentOffset);
	}

	Observable<int> _eyeExpressionNum = new();
	Observable<int> _mouthExpressionNum = new();
	Observable<bool> _mirror = new();
	Observable<bool> _shadow = new(true);
	Observable<bool> _verticalPositioning = new();
	Observable<PoseId> _pose = new();
	Observable<PupilOffsets> _pupilData;

	public string Name { get; }
	public int EyeExpressionNum { get => _eyeExpressionNum.Val; set => _eyeExpressionNum.Val = value; }
	public int MouthExpressionNum { get => _mouthExpressionNum.Val; set => _mouthExpressionNum.Val = value; }
	public bool Mirror { get => _mirror.Val; set => _mirror.Val = value; }
	public bool Shadow { get => _shadow.Val; set => _shadow.Val = value; }
	public bool VerticalPositioning { get => _verticalPositioning.Val; set => _verticalPositioning.Val = value; }
	public PoseId Pose { get => _pose.Val; set => _pose.Val = value; }
	public PupilOffsets PupilData { get => _pupilData.Val; set => _pupilData.Val = value; }
}
