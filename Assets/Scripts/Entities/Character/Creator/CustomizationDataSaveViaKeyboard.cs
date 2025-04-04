using UnityEngine;

namespace Character.Creator
{
    public class CustomizationDataSaveViaKeyboard : MonoBehaviour
    {
        [SerializeField] private SoundEffect _soundEffect;
        private IAudioPlayer _audioPlayer;
        private ICustomizationDiskIO _dataSaver;

        private void Awake()
        {
            _audioPlayer = Singletons.GetSingleton<IAudioPlayer>();
            _dataSaver = this.GetComponent<ICustomizationDiskIO>();
        }

        void Update()
        {
            // Manual saving too
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.S))
            {
                _dataSaver.SaveSelected();
                _audioPlayer.Play(_soundEffect);
            }
        }

        // Not even thinking about auto save yet lul
        // Maybe I'll add it in as an option but there's just too much risk of accidental overriding
    }
}
