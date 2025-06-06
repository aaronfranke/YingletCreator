using Reactivity;

public class HoverCircle : ReactiveBehaviour
{
	private IColliderHoverManager _hoverManager;

	private void Awake()
	{
		_hoverManager = Singletons.GetSingleton<IColliderHoverManager>();
		AddReflector(ReflectHovered);
	}

	private void ReflectHovered()
	{
		var hovered = _hoverManager.CurrentlyHovered;
		this.gameObject.SetActive(hovered != null);
	}

	void LateUpdate()
	{
		var hovered = _hoverManager.CurrentlyHovered;
		if (hovered == null) return;
		this.transform.position = hovered.transform.position;
		this.transform.localScale = hovered.transform.localScale;
	}
}
