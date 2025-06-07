using Character.Creator.UI;
using UnityEngine;

public class PlaySoundOnPoseYingletClicked : MonoBehaviour
{
	[SerializeField] private SoundEffect _selected;
	[SerializeField] private SoundEffect _unselected;
	private IAudioPlayer _audioPlayer;
	private IPoseYingletClicking _poseClicking;

	private void Awake()
	{
		_audioPlayer = Singletons.GetSingleton<IAudioPlayer>();
		_poseClicking = this.GetComponent<IPoseYingletClicking>();
		_poseClicking.OnPoseYingChanged += OnPoseYingChanged;
	}
	private void OnDestroy()
	{
		_poseClicking.OnPoseYingChanged -= OnPoseYingChanged;
	}

	private void OnPoseYingChanged(PoseYing ying)
	{
		var soundEffect = ying == null ? _unselected : _selected;
		_audioPlayer.Play(soundEffect);
	}
}
