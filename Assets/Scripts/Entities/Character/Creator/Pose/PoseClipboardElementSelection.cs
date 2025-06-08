using Character.Creator.UI;
using Reactivity;

/// <summary>
/// The pose page is special in that it's actually also comprised of pages for the individual yings
/// This provides an alternative implementation of ISelectable that accounts for that
/// </summary>
public class PoseClipboardElementSelection : ReactiveBehaviour, ISelectable
{
	Computed<bool> _isSelected;
	private IClipboardElementSelection _clipboardElementSelection;
	private IPoseData _poseData;

	public IReadOnlyObservable<bool> Selected => _isSelected;

	private void Awake()
	{
		_poseData = this.GetComponentInParent<IPoseData>();
		_clipboardElementSelection = this.GetComponent<IClipboardElementSelection>();
		_isSelected = CreateComputed<bool>(ComputeIsSelected);
	}

	private bool ComputeIsSelected()
	{
		return _clipboardElementSelection.Selected.Val && _poseData.CurrentlyEditing == null;
	}
}
