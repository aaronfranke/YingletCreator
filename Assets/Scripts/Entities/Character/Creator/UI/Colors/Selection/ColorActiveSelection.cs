using Character.Data;
using Reactivity;
using System.Collections.Generic;
using System.Linq;

namespace Character.Creator.UI
{
	public interface IColorActiveSelection
	{
		void ToggleSelection(ReColorId id, bool union);
		bool CheckSelected(ReColorId id);
		bool AnySelected { get; }
		IEnumerable<ReColorId> AllSelected { get; }
		ReColorId FirstSelected { get; }
	}

	public class ColorActiveSelection : ReactiveBehaviour, IColorActiveSelection
	{
		ObservableHashSet<ReColorId> _selection = new ObservableHashSet<ReColorId>();
		private ColorSelectionSorter _sorter;
		private Computed<IEnumerable<ReColorId>> _sortedSelected;
		private Computed<ReColorId> _firstSelected;

		void Awake()
		{
			_sorter = this.GetComponent<ColorSelectionSorter>();
			_sortedSelected = CreateComputed(ComputeSortedSelected);
			_firstSelected = CreateComputed(() => _sortedSelected.Val.FirstOrDefault());
		}

		private IEnumerable<ReColorId> ComputeSortedSelected()
		{
			var unsorted = _selection.ToList();
			return _sorter.Sort(unsorted).ToArray();
		}

		public bool AnySelected => _firstSelected.Val != null;

		public IEnumerable<ReColorId> AllSelected => _sortedSelected.Val;

		public ReColorId FirstSelected => _firstSelected.Val;

		public bool CheckSelected(ReColorId id)
		{
			return _selection.Contains(id);
		}

		public void ToggleSelection(ReColorId id, bool union)
		{
			if (union)
			{
				if (_selection.Contains(id))
				{
					_selection.Remove(id);
				}
				else
				{
					_selection.Add(id);
				}
			}
			else
			{
				_selection.Clear();
				_selection.Add(id);
			}
		}
	}
}