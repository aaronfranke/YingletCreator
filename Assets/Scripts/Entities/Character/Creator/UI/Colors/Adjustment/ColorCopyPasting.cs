using Character.Compositor;
using Reactivity;
using System;
using System.Linq;
using UnityEngine;

namespace Character.Creator.UI
{
	public interface IColorCopyPasting
	{
		void Copy();
		void Paste();
		event Action Copied;
		event Action Pasted;
		event Action PasteFailedInvalidFormat;
	}
	public class ColorCopyPasting : MonoBehaviour, IColorCopyPasting
	{
		private ICustomizationSelectedDataRepository _dataRepository;
		private IColorActiveSelection _activeSelection;

		public event Action Copied = delegate { };
		public event Action Pasted = delegate { };
		public event Action PasteFailedInvalidFormat = delegate { };

		void Awake()
		{
			_dataRepository = GetComponentInParent<ICustomizationSelectedDataRepository>();
			_activeSelection = this.GetComponent<IColorActiveSelection>();
		}
		void Update()
		{
			if (!Input.GetKey(KeyCode.LeftControl)) return;
			if (Input.GetKeyDown(KeyCode.C))
			{
				Copy();
			}
			if (Input.GetKeyDown(KeyCode.V))
			{
				Paste();
			}

		}
		public void Copy()
		{
			var id = _activeSelection.FirstSelected;
			if (!id) return;

			var color = _dataRepository.GetColorizeValues(id).Base.GetColor();
			var hexString = "#" + ColorUtility.ToHtmlStringRGB(color);
			GUIUtility.systemCopyBuffer = hexString;

			Copied();
		}


		public void Paste()
		{
			var clipboardColor = GetColorFromClipboard();
			if (clipboardColor == null)
			{
				PasteFailedInvalidFormat();
				return;
			}

			var ids = _activeSelection.AllSelected.ToList();
			using var suspender = new ReactivitySuspender();
			foreach (var id in ids)
			{
				var existingColor = _dataRepository.GetColorizeValues(id);
				var newColors = new ColorizeValuesFromRealColor(clipboardColor.Value, existingColor);
				_dataRepository.SetColorizeValues(id, newColors);
			}
			Pasted();

		}

		public Color? GetColorFromClipboard()
		{
			string clipboardText = GUIUtility.systemCopyBuffer;
			if (string.IsNullOrWhiteSpace(clipboardText)) return null;
			if (!clipboardText.StartsWith("#")) clipboardText = "#" + clipboardText;
			if (ColorUtility.TryParseHtmlString(clipboardText, out Color color))
			{
				return color;
			}
			return null;
		}
	}

}