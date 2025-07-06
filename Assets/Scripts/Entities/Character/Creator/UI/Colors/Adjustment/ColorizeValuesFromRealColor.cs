using Character.Compositor;
using UnityEngine;

namespace Character.Creator.UI
{
	internal class ColorizeValuesFromRealColor : IColorizeValues
	{
		private readonly IColorizeValuesPart _base;
		private readonly IColorizeValuesPart _shade;

		public ColorizeValuesFromRealColor(Color color, IColorizeValues existing)
		{
			_base = new ColorizeValuesFromRealColorPart(color);
			_shade = new ColorizeValuesFromRealColorPart(color, existing);
		}

		public IColorizeValuesPart Base => _base;

		public IColorizeValuesPart Shade => _shade;
	}
	internal class ColorizeValuesFromRealColorPart : IColorizeValuesPart
	{
		public ColorizeValuesFromRealColorPart(Color color)
		{
			Color.RGBToHSV(color, out var hue, out var saturation, out var value);
			Hue = hue;
			Saturation = saturation;
			Value = value;
		}
		public ColorizeValuesFromRealColorPart(Color color, IColorizeValues existing)
		{
			Color.RGBToHSV(color, out var hue, out var saturation, out var value);
			Hue = (hue + existing.Shade.Hue - existing.Base.Hue).Wrap01();
			Saturation = (saturation + existing.Shade.Saturation - existing.Base.Saturation);
			Value = (value + existing.Shade.Value - existing.Base.Value);
		}

		public float Hue { get; }

		public float Saturation { get; }

		public float Value { get; }
	}
}
