using Reactivity;
using TMPro;


public class YingletHeightDisplay : ReactiveBehaviour
{
	private IYingletHeightProvider _heightProvider;
	private TMP_Text _text;

	Observable<float> _rawHeight = new Observable<float>(0f);

	void Start()
	{
		_heightProvider = this.GetCharacterCreatorComponent<IYingletHeightProvider>();
		_text = this.GetComponent<TMP_Text>();
		AddReflector(ReflectText);
	}

	private void LateUpdate()
	{
		_rawHeight.Val = _heightProvider.YScale;
	}

	void ReflectText()
	{
		float val = _rawHeight.Val;
		_text.text = $"{val.ToString("F2")}m";
	}


}
