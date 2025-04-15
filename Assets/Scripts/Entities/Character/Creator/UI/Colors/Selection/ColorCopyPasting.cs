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
			if (_copiedValue == null) return;

			var ids = _activeSelection.AllSelected.ToList();

			using var suspender = new ReactivitySuspender();
			foreach (var id in ids)
			{
				_dataRepository.SetColorizeValues(id, _copiedValue);
			}
			Pasted();
		}
	}
}