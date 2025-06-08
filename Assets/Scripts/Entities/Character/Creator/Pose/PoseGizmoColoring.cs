using Reactivity;
using UnityEngine;

public class PoseGizmoColoring : ReactiveBehaviour
{
	[SerializeField] Color _inactiveColor;
	[SerializeField] Color _hoveredColor;
	[SerializeField] Color _draggedColor;
	[SerializeField] SharedEaseSettings _easeSettings;

	private SpriteRenderer _spriteRenderer;
	private IHoverable _hoverable;
	private IPoseGizmo _poseGizmo;
	private Coroutine _transitionCoroutine;

	private void Awake()
	{
		_spriteRenderer = this.GetComponent<SpriteRenderer>();
		_hoverable = this.GetComponentInParent<IHoverable>();
		_poseGizmo = this.GetComponentInParent<IPoseGizmo>();
	}

	void Start()
	{
		AddReflector(Reflect);
	}

	private void Reflect()
	{
		Color from = _spriteRenderer.color;
		Color to = _inactiveColor;
		if (_poseGizmo.Dragging)
		{
			to = _draggedColor;
		}
		else if (_hoverable.Hovered.Val)
		{
			to = _hoveredColor;
		}
		this.StartEaseCoroutine(ref _transitionCoroutine, _easeSettings, p => _spriteRenderer.color = Color.LerpUnclamped(from, to, p));
	}
}
