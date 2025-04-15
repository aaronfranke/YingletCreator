using Character.Compositor;
using Reactivity;
using System.Linq;
using UnityEngine;

namespace Character.Creator.UI
{
	public interface IColorCopyPasting
	{
		void Copy();
		void Paste();
	}
	public class ColorCopyPasting : MonoBehaviour, IColorCopyPasting
	{
		private ICustomizationSelectedDataRepository _dataRepository;
		private IColorActiveSelection _activeSelection;
		private IColorizeValues _copiedValue;

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
		}
	}
}