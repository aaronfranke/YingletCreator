using Reactivity;

public class PoseYingHoverCircle : ReactiveBehaviour
{
	private IPoseData _poseData;
	private IHoveredPoseYingProvider _activelyHoveredProvider;

	Computed<bool> _shouldShowThis;

	private void Awake()
	{
		_poseData = this.GetComponentInParent<IPoseData>();
		_activelyHoveredProvider = this.GetComponentInParent<IHoveredPoseYingProvider>();
		_shouldShowThis = this.CreateComputed(ShouldShowThis);
		AddReflector(ReflectHovered);
	}

	private bool ShouldShowThis()
	{
		var hovered = _activelyHoveredProvider.HoveredPoseYing;
		if (hovered == null)
		{
			return false;
		}

		bool hoveringOverEditing = _poseData.CurrentlyEditing?.Reference == hovered.Reference;
		return !hoveringOverEditing;
	}

	private void ReflectHovered()
	{
		this.gameObject.SetActive(_shouldShowThis.Val);
	}

	void LateUpdate()
	{
		if (!_shouldShowThis.Val) return;
		var hovered = _activelyHoveredProvider.HoveredPoseYing;
		this.transform.position = hovered.GameObject.transform.position;
		this.transform.localScale = hovered.GameObject.transform.localScale;
	}
}

