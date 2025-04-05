using Reactivity;
using TMPro;

// Not an image yet until we have the technology

namespace Character.Creator.UI
{
    public class YingPortraitImage : ReactiveBehaviour
    {
        private ICustomizationSelection _selection;
        private IYingPortraitReference _reference;
        private TMP_Text _text;

        private void Awake()
        {
            _reference = this.GetComponentInParent<IYingPortraitReference>();
            _text = this.GetComponent<TMP_Text>();

        }

        private void Start()
        {
            AddReflector(ReflectImage);
        }

        void ReflectImage()
        {
            if (_reference.Reference == null) return;
            _text.text = _reference.Reference.CachedData.Name;
        }
    }
}