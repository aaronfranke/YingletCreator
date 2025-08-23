using UnityEngine;
using UnityEngine.UI;

public class ScreenTransitioner : MonoBehaviour
{
	[SerializeField] EaseSettings _easeSettings;
	[SerializeField] Vector2 _transitionRange = new Vector2(0, 1);

	private Coroutine _coroutine;
	private Image _image;

	void Start()
	{
		_image = this.GetComponent<Image>();
		TransitionToClear();
	}

	void TransitionToClear()
	{
		_image.enabled = true;
		float from = Mathf.Min(_transitionRange.y, 1);
		float to = Mathf.Max(_transitionRange.x, 0);

		this.StartEaseCoroutine(ref _coroutine, _easeSettings, Apply, OnComplete);
		void Apply(float p)
		{
			_image.material.SetFloat("_Value", Mathf.Lerp(from, to, p));
		}
		void OnComplete()
		{
			_image.enabled = false;
		}
	}
}
