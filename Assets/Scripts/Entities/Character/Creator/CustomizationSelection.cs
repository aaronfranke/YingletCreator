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
        public CachedYingletReference Selected { get; set; }
    }

    public class CustomizationSelection : MonoBehaviour, ICustomizationSelection
    {
        private ICustomizationYingletRepository _yingletRepository;

        private Observable<CachedYingletReference> _selected = new Observable<CachedYingletReference>();

        void Awake()
        {
            _yingletRepository = this.GetComponent<ICustomizationYingletRepository>();

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
            set
            {
                _selected.Val = value;
            }
        }
    }

}