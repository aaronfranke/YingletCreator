using Character.Creator.UI;
using Reactivity;
using UnityEngine;

enum PoseClipboardElementPageType
{
	Base,
	YingEven,
	YingOdd
}

/// <summary>
/// The pose page is special in that it's actually also comprised of pages for the individual yings
/// This provides an alternative implementation of ISelectable that accounts for that
/// </summary>
public class PoseClipboardElementSelection : ReactiveBehaviour, ISelectable
{
	[SerializeField] PoseClipboardElementPageType _type;

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
		bool poseBookmarkSelected = _clipboardElementSelection.Selected.Val;
		if (!poseBookmarkSelected) return false;


		bool isEditingSomething = _poseData.CurrentlyEditing != null;
		if (_type == PoseClipboardElementPageType.Base)
		{
			return !isEditingSomething;
		}
		else if (_type == PoseClipboardElementPageType.YingEven)
		{
			return isEditingSomething && _poseData.EditingEven;
		}
		else /* Odd */
		{
			return isEditingSomething && !_poseData.EditingEven;

		}
	}
}
