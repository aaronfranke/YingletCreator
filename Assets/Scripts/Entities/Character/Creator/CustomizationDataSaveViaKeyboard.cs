using UnityEngine;

namespace Character.Creator
{
    public class CustomizationDataSaveViaKeyboard : MonoBehaviour
    {
        [SerializeField] private SoundEffect _soundEffect;
        private IAudioPlayer _audioPlayer;
        private ICustomizationDataSaver _dataSaver;

        private void Awake()
        {
            _audioPlayer = Singletons.GetSingleton<IAudioPlayer>();
            _dataSaver = this.GetComponent<ICustomizationDataSaver>();
        }

        void Update()
        {
            // Manual saving too
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.S))
            {
                _dataSaver.SaveCurrent();
                _audioPlayer.Play(_soundEffect);
            }
        }

        // Not even thinking about auto save yet lul
        // Maybe I'll add it in as an option but there's just too much risk of accidental overriding
    }
}
