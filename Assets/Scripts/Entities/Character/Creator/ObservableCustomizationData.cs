using Character.Data;
using Reactivity;
using System.Collections.Generic;

namespace Character.Creator
{


    public class ObservableCustomizationData : ICustomizationData
    {
        public ICustomizationSliderData SliderData { get; } = new ObervableCustomizationSliderData();
    }

    internal class ObervableCustomizationSliderData : ICustomizationSliderData
    {
        readonly ObservableDict<CharacterSliderId, Observable<float>> _sliderValues;
        private readonly DictWithObservableValuesAbstraction<CharacterSliderId, float> _dictAbstraction;

        public ObervableCustomizationSliderData()
        {
            _sliderValues = new ObservableDict<CharacterSliderId, Observable<float>>();
            _dictAbstraction = new DictWithObservableValuesAbstraction<CharacterSliderId, float>(_sliderValues);
        }

        public IDictionary<CharacterSliderId, float> SliderValues => _dictAbstraction;
    }
}
