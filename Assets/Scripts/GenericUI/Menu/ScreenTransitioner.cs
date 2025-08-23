using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public interface IScreenTransitioner
{
	void TransitionToOpaque();
	float TransitionTime { get; }
}

public class ScreenTransitioner : MonoBehaviour, IScreenTransitioner
{
	[SerializeField] private SoundEffect _soundEffect;
	[SerializeField] EaseSettings _easeSettings;
	[SerializeField] Vector2 _transitionRange = new Vector2(0, 1);

	private Coroutine _coroutine;
	private IAudioPlayer _audioPlayer;
	private Image _image;

	public float TransitionTime => _easeSettings.Duration;

	IEnumerator Start()
	{
		_audioPlayer = Singletons.GetSingleton<IAudioPlayer>();
		_image = this.GetComponent<Image>();

		_image.enabled = true;
		SetVal(1);
		yield return null;
		TransitionToClear();
	}

	void TransitionToClear()
	{
		_image.enabled = true;
		_image.material.SetFloat("_X", 1);
		_audioPlayer.Play(_soundEffect);

		float from = _transitionRange.y;
		float to = _transitionRange.x;

		this.StartEaseCoroutine(ref _coroutine, _easeSettings, Apply, OnComplete);
		void Apply(float p)
		{
			SetVal(Mathf.Lerp(from, to, p));
		}
		void OnComplete()
		{
			_image.enabled = false;
		}
	}
	public void TransitionToOpaque()
	{
		_image.enabled = true;
		_image.material.SetFloat("_X", -1);
		_audioPlayer.Play(_soundEffect);

		float from = _transitionRange.x;
		float to = _transitionRange.y;


		this.StartEaseCoroutine(ref _coroutine, _easeSettings, Apply);
		void Apply(float p)
		{
			SetVal(Mathf.Lerp(from, to, p));
		}
	}

	private void SetVal(float v)
	{
		_image.material.SetFloat("_Value", v);
	}

}
