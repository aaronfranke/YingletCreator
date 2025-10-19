using Reactivity;
using TMPro;

public class PoseCameraSpeedText : ReactiveBehaviour
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
		_text.text = $"Cam Speed ({_camData.Speed:F1}x)";
	}
}
