using System.Collections;
using UnityEngine;

public class PlaySoundAfterFrame : MonoBehaviour
{
	[SerializeField] private SoundEffect _soundEffect;

	private IEnumerator Start()
	{
		yield return null;
		var audioPlayer = Singletons.GetSingleton<IAudioPlayer>();
		audioPlayer.Play(_soundEffect);
	}

}
