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
	}
	public class ColorCopyPasting : MonoBehaviour, IColorCopyPasting
	{
		private ICustomizationSelectedDataRepository _dataRepository;
		private IColorActiveSelection _activeSelection;
		private IColorizeValues _copiedValue;

		public event Action Copied = delegate { };
		public event Action Pasted = delegate { };

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

			_copiedValue = _dataRepository.GetColorizeValues(id);
			Copied();
		}

		public void Paste()
		{
			var value = _copiedValue;

			// Little hack to paste real colors in from hex codes; might make this real in the future
			if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.LeftAlt))
			{
				var colorFromClipboard = GetColorFromClipboard();
				if (colorFromClipboard != null)
				{
					value = new ColorizeValuesFromRealColor((Color)colorFromClipboard);
				}
			}

			if (value == null) return;

			var ids = _activeSelection.AllSelected.ToList();

			using var suspender = new ReactivitySuspender();
			foreach (var id in ids)
			{
				_dataRepository.SetColorizeValues(id, value);
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
	class ColorizeValuesFromRealColor : IColorizeValues
	{
		private readonly IColorizeValuesPart _part;

		public ColorizeValuesFromRealColor(Color color)
		{
			_part = new ColorizeValuesFromRealColorPart(color);
		}

		public IColorizeValuesPart Base => _part;

		public IColorizeValuesPart Shade => _part;
	}
	class ColorizeValuesFromRealColorPart : IColorizeValuesPart
	{
		public ColorizeValuesFromRealColorPart(Color color)
		{
			Color.RGBToHSV(color, out var hue, out var saturation, out var value);
			Hue = hue;
			Saturation = saturation;
			Value = value;
		}

		public float Hue { get; }

		public float Saturation { get; }

		public float Value { get; }
	}
}