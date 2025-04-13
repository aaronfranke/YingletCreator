using Character.Data;
using Reactivity;
using UnityEngine;

namespace Character.Creator.UI
{
	public interface IColorActiveSelection
	{
		void ToggleSelection(ReColorId id, bool union);
		bool CheckSelected(ReColorId id);
	}

	public class ColorActiveSelection : MonoBehaviour, IColorActiveSelection
	{
		ObservableHashSet<ReColorId> _selection = new ObservableHashSet<ReColorId>();

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