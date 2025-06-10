using Character.Creator;
using Reactivity;

/// <summary>
/// Pose data for an individual ying
/// For all the pose data, refer to IPoseData
/// </summary>
public interface IYingPoseData
{
	string Name { get; }
	int MouthExpressionNum { get; set; }
	int EyeExpressionNum { get; set; }
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
	}

	// TODO: Pass in the observable data into the constructor and get the defaults
	Observable<int> _mouthExpressionNum = new();
	Observable<int> _eyeExpressionNum = new();

	public string Name { get; }
	public int MouthExpressionNum { get => _mouthExpressionNum.Val; set => _mouthExpressionNum.Val = value; }
	public int EyeExpressionNum { get => _eyeExpressionNum.Val; set => _eyeExpressionNum.Val = value; }

}
