using Reactivity;
using System.Linq;
using System.Text;
using TMPro;

namespace Character.Creator.UI
{
	public class ReflectColorSelectionText : ReactiveBehaviour
	{
		private IColorActiveSelection _activeSelection;
		private ILightDarkSelection _lightDarkSelection;
		private TMP_Text _text;

		private void Awake()
		{
			_activeSelection = this.GetComponentInParent<IColorActiveSelection>();
			_lightDarkSelection = this.GetComponentInParent<ILightDarkSelection>();
			_text = this.GetComponent<TMP_Text>();
		}

		private void Start()
		{
			AddReflector(Reflect);
		}

		private void Reflect()
		{
			var allSelected = _activeSelection.AllSelected.ToArray();
			var firstSelected = allSelected.FirstOrDefault();
			if (firstSelected == null) return;
			var sb = new StringBuilder();
			sb.Append("Editing");
			if (!_lightDarkSelection.Light) sb.Append(" (Shade)");
			sb.Append(": ");
			sb.Append(firstSelected.name);
			if (allSelected.Length > 1)
			{
				sb.Append($" and {allSelected.Length - 1} other(s)");
			}
			_text.text = sb.ToString();
		}
	}
}