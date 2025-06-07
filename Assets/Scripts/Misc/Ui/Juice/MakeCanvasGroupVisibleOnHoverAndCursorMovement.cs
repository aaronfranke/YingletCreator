using Reactivity;
using UnityEngine;

public class MakeCanvasGroupVisibleOnHoverAndCursorMovement : ReactiveBehaviour
{
	// Let this become visible based on something else's hover state
	[SerializeField] UiHoverable _hoverable;
	[SerializeField] SharedEaseSettings _easeSettings;
	private CanvasGroup _canvasGroup;
	private Coroutine _transitionCoroutine;
	private Observable<bool> _cursorMovedRecently = new(false);
	private Computed<bool> _shouldBeVisible;
	private Vector3 _lastMousePosition;
	private float _lastMovementTime;
	const float CURSOR_MOVE_TIME = 1.8f;

	private void Awake()
	{
		_canvasGroup = this.GetComponent<CanvasGroup>();
		_canvasGroup.alpha = 0;
	}

	void Start()
	{
		_shouldBeVisible = this.CreateComputed(CreateComputed);
		AddReflector(Reflect);
	}

	void Update()
	{
		Vector3 currentMousePosition = Input.mousePosition;

		if (currentMousePosition != _lastMousePosition)
		{
			_lastMovementTime = Time.time;
			_lastMousePosition = currentMousePosition;
		}

		_cursorMovedRecently.Val = (Time.time - _lastMovementTime) <= CURSOR_MOVE_TIME;
	}

	private bool CreateComputed()
	{
		return _hoverable.Hovered.Val && _cursorMovedRecently.Val;
	}

	private void Reflect()
	{
		float from = _canvasGroup.alpha;
		float to = _shouldBeVisible.Val ? 1 : 0;
		this.StartEaseCoroutine(ref _transitionCoroutine, _easeSettings, p => _canvasGroup.alpha = Mathf.LerpUnclamped(from, to, p));
	}
}
