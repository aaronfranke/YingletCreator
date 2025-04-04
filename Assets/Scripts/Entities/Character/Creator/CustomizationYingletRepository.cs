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
    }

    public class CustomizationYingletRepository : MonoBehaviour, ICustomizationYingletRepository
    {
        private ICustomizationDiskIO _diskIO;

        private Dictionary<CustomizationYingletGroup, ObservableList<CachedYingletReference>> _yinglets = new();

        public IEnumerable<CachedYingletReference> GetYinglets(CustomizationYingletGroup group)
        {
            return _yinglets[group];
        }

        private void Awake()
        {
            _diskIO = this.GetComponent<ICustomizationDiskIO>();
            LoadAllYinglets();
        }

        void LoadAllYinglets()
        {
            LoadGroupYinglets(CustomizationYingletGroup.Preset);
            LoadGroupYinglets(CustomizationYingletGroup.Custom);

            void LoadGroupYinglets(CustomizationYingletGroup group)
            {
                var paths = _diskIO.LoadInitialYingData(group).ToArray();
                var list = new ObservableList<CachedYingletReference>();
                foreach (var path in paths)
                {
                    list.Add(path);
                }
                _yinglets[group] = list;
            }
        }
    }
}
