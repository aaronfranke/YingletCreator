using Reactivity;

internal interface IPageYingPoseData
{
	/// <summary>
	/// The data that should be displayed for the given page
	/// This may be null if this page is not displaying the currently editing ying
	/// That's relevant because there's always two pages that swap
	/// It's the consumers responsibility to respect a null data value
	/// </summary>
	IYingPoseData Data { get; }
}

internal sealed class PageYingPoseData : ReactiveBehaviour, IPageYingPoseData
{
	private IPoseData _poseData;
	Computed<IYingPoseData> _dataComputed;

	public IYingPoseData Data => _dataComputed.Val;

	void Awake()
	{
		_poseData = this.GetComponentInParent<IPoseData>();
		_dataComputed = CreateComputed(ComputeData);
	}

	private IYingPoseData ComputeData()
	{
		// TODO: consume even/odd and keep the last data
		var currentlyEditing = _poseData.CurrentlyEditing;

		if (currentlyEditing == null) return null;

		var allData = _poseData.Data;

		allData.TryGetValue(currentlyEditing.Reference, out var data);

		return data;
	}
}
