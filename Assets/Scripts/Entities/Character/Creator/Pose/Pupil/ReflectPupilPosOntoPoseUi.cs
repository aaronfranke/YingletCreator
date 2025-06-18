using Reactivity;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ReflectPupilPosOntoPoseUi : ReactiveBehaviour
{
	private IPosePupilUiData _pupilData;
	private Image _image;
	private Material _material;
	static readonly int OFFSET_PROP_ID = Shader.PropertyToID("_Offset");

	void Awake()
	{
		_pupilData = GetComponent<IPosePupilUiData>();
		_image = this.GetComponent<Image>();
		_material = new Material(_image.material);
		_image.material = _material;
	}

	private void Start()
	{
		AddReflector(Reflect);
	}

	private void Reflect()
	{
		Vector2 offset = _pupilData.PupilPosition;
		_material.SetVector(OFFSET_PROP_ID, offset);
		_image.SuperDirtyMaterial();
	}
}
