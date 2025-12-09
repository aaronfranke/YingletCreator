using UnityEngine;
using UnityEngine.EventSystems;

public interface ITooltip
{
	string Text { get; }
	RectTransform RectTransform { get; }
}

public class Tooltip : MonoBehaviour, ITooltip, IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField, TextArea] string _text;
	private ITooltipManager _tooltipManager;

	public string Text => _text;

	public RectTransform RectTransform { get; private set; }

	private void Awake()
	{
		RectTransform = GetComponent<RectTransform>();
		_tooltipManager = Singletons.GetSingleton<ITooltipManager>();
		if (string.IsNullOrWhiteSpace(_text))
		{
			Debug.LogWarning($"Tooltip on GameObject '{gameObject.name}' has no text set.", this);
		}
	}

	private void OnDestroy()
	{
		_tooltipManager.Unregister(this);
	}

	private void OnDisable()
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
