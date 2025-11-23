using Reactivity;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Character.Creator
{
	/// <summary>
	/// The group the yinglet belongs to.
	/// This determines where it shows up on the main screen,
	/// and enables / disables certain behavior
	/// </summary>
	public enum CustomizationYingletGroup
	{
		Preset,
		Custom,
		// Autosave(?)
	}

	public interface ICustomizationYingletRepository
	{
		IEnumerable<CachedYingletReference> GetYinglets(CustomizationYingletGroup group);
		void AddNewCustom(CachedYingletReference reference);

		/// <summary>
		/// Removes the given yinglet from the custom repository, returning the index it existed at
		/// </summary>
		int DeleteCustom(CachedYingletReference reference);
	}

	public class CustomizationYingletRepository : MonoBehaviour, ICustomizationYingletRepository
	{

		private Dictionary<CustomizationYingletGroup, ObservableList<CachedYingletReference>> _yinglets = new();

		public IEnumerable<CachedYingletReference> GetYinglets(CustomizationYingletGroup group)
		{
			return _yinglets[group];
		}

		private void Awake()
		{
			LoadAllYinglets();
		}

		void LoadAllYinglets()
		{
			var dataLoader = this.GetComponent<IStartupYingletDataLoader>();
			LoadGroupYinglets(CustomizationYingletGroup.Preset);
			LoadGroupYinglets(CustomizationYingletGroup.Custom);

			void LoadGroupYinglets(CustomizationYingletGroup group)
			{
				var paths = dataLoader.LoadInitialYingData(group).ToArray();
				var list = new ObservableList<CachedYingletReference>();
				foreach (var path in paths)
				{
					list.Add(path);
				}
				_yinglets[group] = list;
			}
		}

		public void AddNewCustom(CachedYingletReference reference)
		{
			_yinglets[CustomizationYingletGroup.Custom].Add(reference);
		}

		public int DeleteCustom(CachedYingletReference reference)
		{
			var list = _yinglets[CustomizationYingletGroup.Custom];
			int index = list.IndexOf(reference);
			if (index < 0)
			{
				return -1;
			}
			list.RemoveAt(index);
			return index;
		}
	}
}
