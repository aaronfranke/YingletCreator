using UnityEngine;
using UnityEngine.EventSystems;

public interface ITooltip
{
	string Text { get; }
	RectTransform RectTransform { get; }
}

public class Tooltip : MonoBehaviour, ITooltip, IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField] string _text;
	private ITooltipManager _tooltipManager;

	public string Text => _text;

	public RectTransform RectTransform { get; private set; }

	private void Awake()
	{
		RectTransform = GetComponent<RectTransform>();
		_tooltipManager = Singletons.GetSingleton<ITooltipManager>();
	}

	private void OnDestroy()
	{
		_tooltipManager.Unregister(this);
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		_tooltipManager.Register(this);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		_tooltipManager.Unregister(this);
	}
}
