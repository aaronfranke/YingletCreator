using UnityEngine;

namespace Character.Creator.UI
{
	public class ColorCopyPastingSounds : MonoBehaviour
	{
		[SerializeField] private SoundEffect _copy;
		[SerializeField] private SoundEffect _paste;
		[SerializeField] private SoundEffect _failed;
		private IAudioPlayer _audioPlayer;
		private IColorCopyPasting _copyPasting;

		private void Awake()
		{
			_audioPlayer = Singletons.GetSingleton<IAudioPlayer>();
			_copyPasting = this.GetComponent<IColorCopyPasting>();
			_copyPasting.Copied += CopyPasting_Copied;
			_copyPasting.Pasted += CopyPasting_Pasted;
			_copyPasting.PasteFailedInvalidFormat += CopyPasting_PasteFailedInvalidFormat;
		}

		private void OnDestroy()
		{
			_copyPasting.Copied -= CopyPasting_Copied;
			_copyPasting.Pasted -= CopyPasting_Pasted;
			_copyPasting.PasteFailedInvalidFormat -= CopyPasting_PasteFailedInvalidFormat;
		}
		private void CopyPasting_Copied()
		{
			_audioPlayer.Play(_copy);
		}
		private void CopyPasting_Pasted()
		{
			_audioPlayer.Play(_paste);
		}
		private void CopyPasting_PasteFailedInvalidFormat()
		{
			_audioPlayer.Play(_failed);
		}
	}
}