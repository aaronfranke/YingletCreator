using System.Linq;

namespace Character.Creator
{
    /// <summary>
    /// Representation of save data that is entirely serializable
    /// </summary>
    [System.Serializable]
    public sealed class SerializableCustomizationData
    {
        public string Name;

        public SerializableCustomizationSliderData SliderData;

        public SerializableCustomizationData(ObservableCustomizationData data)
        {
            Name = data.Name.Val;
            SliderData = new SerializableCustomizationSliderData(data.SliderData);
        }
    }

    [System.Serializable]
    public sealed class SerializableCustomizationSliderData
    {
        public SerializableCustomizationSliderData(ObervableCustomizationSliderData data)
        {
            SliderValues = data.SliderValues.Select(kvp => new SliderFloatPair(kvp.Key.UniqueAssetID, kvp.Value.Val)).ToArray();
        }

        /// <summary>
        /// The float value is always in the range of 0 to 1
        /// </summary>
        public SliderFloatPair[] SliderValues;

        [System.Serializable]
        public sealed class SliderFloatPair
        {
            public SliderFloatPair(string sliderID, float value)
            {
                SliderID = sliderID;
                Value = value;

            }
            public string SliderID;
            public float Value;
        }
    }

}