using Character.Data;
using Reactivity;


namespace Character.Creator.UI
{
	public interface IColorSelectionReference
	{
		public ReColorId Id { get; set; }
	}
	public class ColorSelectionReference : ReactiveBehaviour, IColorSelectionReference
	{
		public ReColorId Id { get; set; }
	}
}