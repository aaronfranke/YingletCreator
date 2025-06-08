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

	private Coroutine _transitionCoroutine;

	private void Awake()
	{
		_spriteRenderer = this.GetComponent<SpriteRenderer>();
		_hoverable = this.GetComponentInParent<IHoverable>();
	}

	void Start()
	{
		AddReflector(Reflect);
	}

	private void Reflect()
	{
		Color from = _spriteRenderer.color;
		Color to = _hoverable.Hovered.Val ? _hoveredColor : _inactiveColor;
		this.StartEaseCoroutine(ref _transitionCoroutine, _easeSettings, p => _spriteRenderer.color = Color.LerpUnclamped(from, to, p));
	}
}
