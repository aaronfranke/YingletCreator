using Character.Compositor;
using System;
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

		public string CreationTimeString;
		public DateTime CreationTime
		{
			get
			{
				if (string.IsNullOrWhiteSpace(CreationTimeString))
				{
					return DateTime.MinValue;
				}
				return DateTime.Parse(CreationTimeString);
			}
			set => CreationTimeString = value.ToString("o");
		}

		public SerializableCustomizationSliderData SliderData;
		public SerializableCustomizationColorData ColorData;
		public SerializableCustomizationToggleData ToggleData;

		public SerializableCustomizationData(ObservableCustomizationData data)
		{
			Name = data.Name.Val;
			CreationTime = data.CreationTime;
			SliderData = new SerializableCustomizationSliderData(data.SliderData);
			ColorData = new SerializableCustomizationColorData(data.ColorData);
			ToggleData = new SerializableCustomizationToggleData(data.ToggleData);

		}
	}

	[System.Serializable]
	public sealed class SerializableCustomizationSliderData
	{
		public SerializableCustomizationSliderData(ObservableCustomizationSliderData data)
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
			public SliderFloatPair(string id, float value)
			{
				Id = id;
				Value = value;

			}
			public string Id;
			public float Value;
		}
	}

	[System.Serializable]
	public sealed class SerializableCustomizationColorData
	{
		public SerializableCustomizationColorData(ObservableCustomizationColorData data)
		{
			ColorizeValues = data.ColorizeValues.Select(kvp => new ColorizeValuesPair(kvp.Key.UniqueAssetID, new(kvp.Value.Val))).ToArray();
		}

		/// <summary>
		/// The float value is always in the range of 0 to 1
		/// </summary>
		public ColorizeValuesPair[] ColorizeValues;

		[System.Serializable]
		public sealed class ColorizeValuesPair
		{
			public ColorizeValuesPair(string id, SerializableColorizeValues values)
			{
				Id = id;
				Values = values;

			}
			public string Id;
			public SerializableColorizeValues Values;
		}
	}

	[System.Serializable]
	public sealed class SerializableCustomizationToggleData
	{
		public SerializableCustomizationToggleData(ObservableCustomizationToggleData data)
		{
			ToggleIds = data.Toggles.Select(t => t.UniqueAssetID).ToArray();
		}

		public string[] ToggleIds;
	}
}