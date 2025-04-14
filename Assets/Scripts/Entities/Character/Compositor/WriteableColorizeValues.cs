using Character.Compositor;

namespace Assets.Scripts.Entities.Character.Compositor
{
	internal class WriteableColorizeValues : IColorizeValues
	{
		public WriteableColorizeValues(IColorizeValues source)
		{
			Base = new WriteableColorizeValuesPart(source.Base);
			Shade = new WriteableColorizeValuesPart(source.Shade);
		}
		public WriteableColorizeValuesPart Base { get; }

		public WriteableColorizeValuesPart Shade { get; }

		IColorizeValuesPart IColorizeValues.Base => Base;
		IColorizeValuesPart IColorizeValues.Shade => Shade;
	}
	public class WriteableColorizeValuesPart : IColorizeValuesPart
	{
		public WriteableColorizeValuesPart(IColorizeValuesPart source)
		{
			Hue = source.Hue;
			Saturation = source.Saturation;
			Value = source.Value;
		}
		public float Hue { get; set; }

		public float Saturation { get; set; }

		public float Value { get; set; }
	}
}
