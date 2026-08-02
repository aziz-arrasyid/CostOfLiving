using System.Collections.Generic;
using PrimeTween;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Game.System;

namespace Main.Menu
{
    public class AnimatedSettings : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private RectTransform logo;
        [SerializeField] private RectTransform character;
        [SerializeField] private RectTransform buttonsPanel;
        [SerializeField] private ScrollRect myScrollRect;
        [SerializeField] private Button audioBtn;
        [SerializeField] private Button controlsBtn;
        [SerializeField] private List<RectTransform> contentState; // 0 = Controls, 1 = Audio
        [SerializeField] private List<Sprite> buttonSpriteState; // 0 = default, 1 = active
        [SerializeField] private TextMeshProUGUI bgmValueText;
        [SerializeField] private TextMeshProUGUI sfxValueText;
        [SerializeField] private Slider bgmSlider;
        [SerializeField] private Slider sfxSlider;

        public enum ButtonActiveState
        {
            Controls, // index 0
            Audio, // index 1
        }

        private ButtonActiveState buttonActiveState;

        private Sequence anim;

        private void Start()
        {
            audioBtn.onClick.AddListener(() => ChangeState(ButtonActiveState.Audio));
            controlsBtn.onClick.AddListener(() => ChangeState(ButtonActiveState.Controls));

            bgmSlider.onValueChanged.AddListener((val) => OnSliderChanged(val, SettingsManager.VolumeState.BGM));
            sfxSlider.onValueChanged.AddListener((val) => OnSliderChanged(val, SettingsManager.VolumeState.SFX));

            UpdateAudioUI(SettingsManager.VolumeState.BGM);
            UpdateAudioUI(SettingsManager.VolumeState.SFX);
        }

        private void ChangeState(ButtonActiveState activeState)
        {
            buttonActiveState = activeState;

            Image controlsImg = controlsBtn.GetComponent<Image>();
            Image audioImg = audioBtn.GetComponent<Image>();


            switch (buttonActiveState)
            {
                case ButtonActiveState.Controls:
                    controlsImg.sprite = buttonSpriteState[1];
                    audioImg.sprite = buttonSpriteState[0];

                    contentState[0].gameObject.SetActive(true);
                    contentState[1].gameObject.SetActive(false);
                    break;
                case ButtonActiveState.Audio:
                    audioImg.sprite = buttonSpriteState[1];
                    controlsImg.sprite = buttonSpriteState[0];

                    contentState[1].gameObject.SetActive(true);
                    contentState[0].gameObject.SetActive(false);
                    break;
            }
        }

        public void UpdateUI(bool isOpen)
        {
            if (anim.isAlive) anim.Stop();

            if (isOpen) ChangeState(ButtonActiveState.Controls);

            anim = Sequence.Create();

            CanvasGroup cgButtonsPanel = buttonsPanel.GetComponent<CanvasGroup>();
            CanvasGroup cgLogo = logo.GetComponent<CanvasGroup>();

            float alpha = isOpen ? 0f : 1f;
            float duration = 0.4f;
            Ease easeSine = isOpen ? Ease.InSine : Ease.OutSine;

            Vector2 characterPosition = isOpen ? new Vector2(-632f, -786f) : new Vector2(-103.47f, -680.2341f);

            Tween moveTween = Tween.UIAnchoredPosition(target: character, endValue: characterPosition, duration: duration - 0.1f,
            ease: easeSine).OnUpdate(target: this, (target, tween) =>
            {
                target.myScrollRect.verticalNormalizedPosition = 1f;
            });

            anim
            .Group(Tween.Alpha(target: cgButtonsPanel, endValue: alpha, duration: duration - 0.1f, ease: easeSine))
            .Group(Tween.Alpha(target: cgLogo, endValue: alpha, duration: duration - 0.1f, ease: easeSine));
        }

        private void OnSliderChanged(float value, SettingsManager.VolumeState volumeState)
        {
            SettingsManager.Instance.OnSliderChanged(value, volumeState);
            UpdateAudioUI(volumeState);
        }

        private void UpdateAudioUI(SettingsManager.VolumeState volumeState)
        {
            Settings settingsLoad = GameManager.Instance.LoadData<Settings>("settings");

            switch (volumeState)
            {
                case SettingsManager.VolumeState.BGM:
                    bgmValueText.text = $"{Mathf.RoundToInt(settingsLoad.BGMVolume * 100)}";
                    bgmSlider.value = settingsLoad.BGMVolume;
                    break;
                case SettingsManager.VolumeState.SFX:
                    sfxValueText.text = $"{Mathf.RoundToInt(settingsLoad.SFXVolume * 100)}";
                    sfxSlider.value = settingsLoad.SFXVolume;
                    break;
            }
        }
    }
}
