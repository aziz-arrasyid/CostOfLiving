using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Game.System
{
    public class SettingsManager : MonoBehaviour
    {
        public static SettingsManager Instance { private set; get; }

        [SerializeField] AudioMixer audioMixer;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public enum VolumeState
        {
            BGM,
            SFX
        }

        public void OnSliderChanged(float value, VolumeState volumeState)
        {
            Settings settingsLoad = GameManager.Instance.LoadData<Settings>("settings");

            switch (volumeState)
            {
                case VolumeState.BGM:
                    settingsLoad.BGMVolume = value; 
                    break;
                case VolumeState.SFX:
                    settingsLoad.SFXVolume = value;
                    break;
            }

            GameManager.Instance.SaveData(settingsLoad, "settings");
            SetAudioMixer(volumeState);
        }

        public void SetAudioMixer(VolumeState volumeState)
        {
            Settings settingsLoad = GameManager.Instance.LoadData<Settings>("settings");
            float audio = volumeState == VolumeState.BGM ? settingsLoad.BGMVolume : settingsLoad.SFXVolume;

            if (audio <= 0.01f)
            {
                audioMixer.SetFloat(volumeState.ToString(), -80f);
            }
            else
            {
                float audioDB = Mathf.Log10(audio) * 20f;
                audioMixer.SetFloat(volumeState.ToString(), audioDB);
            }
        }
    }
}
