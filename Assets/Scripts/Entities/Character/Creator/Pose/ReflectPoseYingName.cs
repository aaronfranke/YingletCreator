using Reactivity;
using TMPro;

public class ReflectPoseYingName : ReactiveBehaviour
{
	private PageYingPoseData _poseData;
	private TMP_Text _text;

	private void Start()
	{
		_poseData = this.GetComponentInParent<PageYingPoseData>();
		_text = this.GetComponent<TMP_Text>();
		AddReflector(Reflect);
	}

	private void Reflect()
	{
		var data = _poseData.Data;
		if (data == null) return;

		_text.text = data.Name;
	}
}
