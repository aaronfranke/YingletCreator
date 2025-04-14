using UnityEngine;


namespace Character.Creator.UI
{
	public class ColorSelectionSounds : MonoBehaviour
	{
		[SerializeField] private SoundEffect _selected;
		[SerializeField] private SoundEffect _unselected;
		private IAudioPlayer _audioPlayer;
		private IColorSelectionClicking _clicking;

		private void Awake()
		{
			_audioPlayer = Singletons.GetSingleton<IAudioPlayer>();
			_clicking = this.GetComponent<IColorSelectionClicking>();
			_clicking.OnChange += Clicking_OnChange;
		}
		private void OnDestroy()
		{
			_clicking.OnChange -= Clicking_OnChange;
		}
		private void Clicking_OnChange(bool selected)
		{
			_audioPlayer.Play(selected ? _selected : _unselected);
		}

	}
}