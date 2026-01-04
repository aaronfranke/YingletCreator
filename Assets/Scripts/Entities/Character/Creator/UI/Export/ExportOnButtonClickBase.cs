using Character.Creator;
using UnityEngine;

public abstract class ExportOnButtonClickBase : MonoBehaviour
{
	private UnityEngine.UI.Button _button;
	private ISaveFolderProvider _saveFolderProvider;
	protected ICustomizationSelection _selection;
	protected Tooltip _tooltip;
	public event System.Action OnExport = delegate { };
	private ICustomizationSelectedDataRepository _dataRepo;
	[SerializeField] private AssetReferenceT<Character.Data.CharacterIntId> _eyeExpressionIntIdReference;
	private ApplySliderAsScaleBase _headSizeApplySlider = null;
	private ApplySliderAsScaleBase _neckSizeApplySlider = null;
	[SerializeField] private GameObject _headSizeApplyGameObject;
	[SerializeField] private AssetReferenceT<Character.Data.CharacterSliderId> _headSizeSliderReference;
	private ApplySliderAsScaleBase _antennaSizeApplySlider = null;
	[SerializeField] private GameObject _antennaSizeApplyGameObject;
	[SerializeField] private AssetReferenceT<Character.Data.CharacterSliderId> _antennaSizeSliderReference;
	private ApplySliderAsScaleBase _earSizeApplySlider = null;
	[SerializeField] private GameObject _earSizeApplyGameObject;
	[SerializeField] private AssetReferenceT<Character.Data.CharacterSliderId> _earSizeSliderReference;
	private ApplySliderAsScaleBase _earLengthApplySlider = null;
	[SerializeField] private GameObject _earLengthApplyGameObject;
	[SerializeField] private AssetReferenceT<Character.Data.CharacterSliderId> _earLengthSliderReference;
	[SerializeField] protected Transform _yingletRoot;
	[SerializeField] protected Transform _skeletonHips;

	protected virtual void Awake()
	{
		_button = this.GetComponent<UnityEngine.UI.Button>();
		_button.onClick.AddListener(OnExportButtonClicked);
		_saveFolderProvider = Singletons.GetSingleton<ISaveFolderProvider>();
		_selection = this.GetCharacterCreatorComponent<ICustomizationSelection>();
		_dataRepo = this.GetComponentInParent<ICustomizationSelectedDataRepository>();
		_tooltip = GetComponent<Tooltip>();
		// These GameObjects have multiple of the same component on them, so we need this ugly code to find the right ones.
		ApplySliderAsScaleBase[] applySliders = _headSizeApplyGameObject.GetComponents<ApplySliderAsScaleBase>();
		foreach (ApplySliderAsScaleBase slider in applySliders)
		{
			if (!slider.ReferencesSliderId(_headSizeSliderReference)) continue;
			if (_headSizeApplySlider == null)
			{
				_headSizeApplySlider = slider;
			}
			else
			{
				_neckSizeApplySlider = slider;
			}
		}
		applySliders = _antennaSizeApplyGameObject.GetComponents<ApplySliderAsScaleBase>();
		foreach (ApplySliderAsScaleBase slider in applySliders)
		{
			if (!slider.ReferencesSliderId(_antennaSizeSliderReference)) continue;
			_antennaSizeApplySlider = slider;
			break;
		}
		applySliders = _earSizeApplyGameObject.GetComponents<ApplySliderAsScaleBase>();
		foreach (ApplySliderAsScaleBase slider in applySliders)
		{
			if (!slider.ReferencesSliderId(_earSizeSliderReference)) continue;
			_earSizeApplySlider = slider;
			break;
		}
		applySliders = _earLengthApplyGameObject.GetComponents<ApplySliderAsScaleBase>();
		foreach (ApplySliderAsScaleBase slider in applySliders)
		{
			if (!slider.ReferencesSliderId(_earLengthSliderReference)) continue;
			_earLengthApplySlider = slider;
			break;
		}
		if (_headSizeApplySlider == null || _neckSizeApplySlider == null || _antennaSizeApplySlider == null || _earSizeApplySlider == null || _earLengthApplySlider == null)
		{
			Debug.LogError("Failed to find one or more ApplySliderAsScaleBase components for export.");
		}
	}

	protected virtual void OnDestroy()
	{
		_button.onClick.RemoveListener(OnExportButtonClicked);
	}

	protected abstract void OnExportButtonClicked();

	protected string GetCharacterNodeName()
	{
		var selected = _selection.Selected;
		var name = (selected != null) ? selected.CachedData.Name : "UnnamedYinglet";
		var sanitizedName = System.Text.RegularExpressions.Regex.Replace(name, "[^a-zA-Z0-9]", "");
		return sanitizedName;
	}

	public EyeExpression GetEyeExpression()
	{
		return (EyeExpression)_dataRepo.GetInt(_eyeExpressionIntIdReference.LoadSync());
	}

	public float GetHeadAntennaeLength()
	{
		float parent = _skeletonHips.lossyScale.y * _neckSizeApplySlider.GetSize().y * _headSizeApplySlider.GetSize().y;
		return parent * (_antennaSizeApplySlider.GetSize().y * 0.15f);
	}

	public float GetHeadEarLength()
	{
		float parent = _skeletonHips.lossyScale.y * _neckSizeApplySlider.GetSize().y * _headSizeApplySlider.GetSize().y;
		return parent * (_earSizeApplySlider.GetSize().y * _earLengthApplySlider.GetSize().y * 0.2f);
	}

	protected string GetSavePath()
	{
		var selected = _selection.Selected;
		var name = (selected != null) ? selected.CachedData.Name : "UnnamedYinglet";
		var sanitizedName = System.Text.RegularExpressions.Regex.Replace(name, "[^a-zA-Z0-9_-]", "");
		var folderName = sanitizedName + "-" + System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
		return System.IO.Path.Combine(_saveFolderProvider.ExportsFolderPath, folderName);
	}

	protected void EmitExportEvent()
	{
		OnExport.Invoke();
	}
}
