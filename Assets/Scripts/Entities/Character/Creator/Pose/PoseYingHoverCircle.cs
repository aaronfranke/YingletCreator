using Reactivity;

public class PoseYingHoverCircle : ReactiveBehaviour
{
	private IHoveredPoseYingProvider _activelyHoveredProvider;

	private void Awake()
	{
		_activelyHoveredProvider = this.GetComponentInParent<IHoveredPoseYingProvider>();
		AddReflector(ReflectHovered);
	}

	private void ReflectHovered()
	{
		var hovered = _activelyHoveredProvider.HoveredPoseYing;
		this.gameObject.SetActive(hovered != null);
	}

	void LateUpdate()
	{
		var hovered = _activelyHoveredProvider.HoveredPoseYing;
		if (hovered == null) return;
		this.transform.position = hovered.GameObject.transform.position;
		this.transform.localScale = hovered.GameObject.transform.localScale;
	}
}

