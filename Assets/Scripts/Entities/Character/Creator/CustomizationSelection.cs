using Reactivity;
using System.Linq;
using UnityEngine;

namespace Character.Creator
{
	/// <summary>
	/// Returns observable data associated to the selected yinglet
	/// </summary>
	public interface ICustomizationSelection
	{
		public CachedYingletReference Selected { get; }

		public void SetSelected(CachedYingletReference reference, bool withConfirmation);

		public bool SelectionIsDirty { get; set; }
	}

	public class CustomizationSelection : MonoBehaviour, ICustomizationSelection
	{
		private ICustomizationYingletRepository _yingletRepository;
		private ICharacterCreatorUndoManager _undoManager;
		private IConfirmationManager _confirmationManager;

		private Observable<CachedYingletReference> _selected = new Observable<CachedYingletReference>();

		void Awake()
		{
			_yingletRepository = this.GetComponent<ICustomizationYingletRepository>();
			_undoManager = this.GetComponentInParent<ICharacterCreatorUndoManager>();
			_confirmationManager = Singletons.GetSingleton<IConfirmationManager>();

			// Try to select first preset, or first custom as a backup
			var initialSelection = _yingletRepository.GetYinglets(CustomizationYingletGroup.Preset).FirstOrDefault();
			if (initialSelection == null) initialSelection = _yingletRepository.GetYinglets(CustomizationYingletGroup.Custom).First();
			_selected.Val = initialSelection;
		}

		public CachedYingletReference Selected
		{
			get
			{
				return _selected.Val;
			}
		}

		public bool SelectionIsDirty { get; set; }

		public void SetSelected(CachedYingletReference reference, bool withConfirmationAndUndo)
		{
			if (withConfirmationAndUndo
				&& _selected.Val.Group == CustomizationYingletGroup.Custom
				&& SelectionIsDirty)
			{
				_confirmationManager.OpenConfirmation(new(
					"Are you sure you want to switch yinglets?\n\nUnsaved changes will be lost.",
					"Discard Changes",
					"change-yinglet-selection",
					SetSelected));
				return;
			}
			else
			{
				SetSelected();
			}

			void SetSelected()
			{
				if (withConfirmationAndUndo)
				{
					_undoManager.RecordState($"Selected yinglet \"{reference.CachedData.Name}\"");
				}
				_selected.Val = reference;
				SelectionIsDirty = false;
			}
		}
	}
}