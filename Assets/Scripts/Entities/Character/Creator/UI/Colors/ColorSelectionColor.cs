using Character.Compositor;
using Reactivity;
using UnityEngine.UI;


namespace Character.Creator.UI
{
	public class ColorSelectionColor : ReactiveBehaviour
	{
		private IColorSelectionReference _reference;
		private Image _image;

		private void Awake()
		{
			_reference = this.GetComponentInParent<IColorSelectionReference>();
			_image = this.GetComponent<Image>();
		}
		private void Start()
		{
			AddReflector(Reflect);
		}

		private void Reflect()
		{
			// Not yet reflecting anything
			_image.color = _reference.Id.ColorGroup.BaseDefaultColor.GetColor();
		}
	}
}