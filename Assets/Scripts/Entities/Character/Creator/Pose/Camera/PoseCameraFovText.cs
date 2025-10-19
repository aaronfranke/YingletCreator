using Reactivity;
using TMPro;

public class PoseCameraFovText : ReactiveBehaviour
{
	private IPoseCameraData _camData;
	private TMP_Text _text;

	void Start()
	{
		_camData = GetComponentInParent<IPoseCameraData>();
		_text = this.GetComponent<TMP_Text>();
		AddReflector(Reflect);
	}

	private void Reflect()
	{
		_text.text = $"Field of View ({_camData.FieldOfView:F0}°)";
	}
}
