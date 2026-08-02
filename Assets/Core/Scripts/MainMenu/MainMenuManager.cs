using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PrimeTween;
using Game.System;
using System.Diagnostics;

namespace Main.Menu
{
    public class MainMenuManager : MonoBehaviour
    {
        [SerializeField] private AnimatedSettings animatedSettings;
        [SerializeField] private AnimatedCredits animatedCredits;

        [Header("UI")]
        [SerializeField] private Button startBtn;
        [SerializeField] private Button settingsBtn;
        [SerializeField] private Button creditsBtn;
        [SerializeField] private Button exitBtn;
        [SerializeField] private RectTransform panel;
        [SerializeField] private Button closePanel;
        [SerializeField] private RectTransform settingsContent;
        [SerializeField] private RectTransform creditsContent;
        [SerializeField] private RectTransform character;
        [SerializeField] private RectTransform logo;
        [SerializeField] private RectTransform mainMenuBtn;

        public enum MainMenuPanelState
        {
            main,
            settings,
            credits
        }

        public MainMenuPanelState mainMenuPanelState;

        private Sequence animPanel;

        private void Start()
        {
            startBtn.onClick.AddListener(OnStartBtnClicked);
            settingsBtn.onClick.AddListener(OnSettingsBtnClicked);
            creditsBtn.onClick.AddListener(OnCreditsBtnClicked);
            exitBtn.onClick.AddListener(OnExitBtnClicked);
            closePanel.onClick.AddListener(OnClosePanel);

            panel.gameObject.SetActive(false);
            character.anchoredPosition = new(-103.47f, -680.2341f);

            character.gameObject.SetActive(true);
            logo.gameObject.SetActive(true);
            mainMenuBtn.gameObject.SetActive(true);
            logo.anchoredPosition = new(561f, -333f);
            logo.localScale = Vector2.one;
        }

        private void IsOpeningPanel(bool status)
        {
            CanvasGroup cg = panel.GetComponent<CanvasGroup>();

            float alpha = status ? 1f : 0f;
            float duration = 0.4f;
            Vector2 scale = status ? Vector2.one : Vector2.zero;
            Ease easeAlpha = status ? Ease.OutSine : Ease.InSine;
            Ease easeScale = status ? Ease.OutQuart : Ease.InQuart;

            cg.alpha = status ? 0f : 1f;
            panel.localScale = status ? Vector2.zero : Vector2.one;
            panel.gameObject.SetActive(true);

            if (animPanel.isAlive) animPanel.Stop();

            animPanel = Sequence.Create();

            animPanel
            .Group(Tween.Alpha(target: cg, endValue: alpha, duration: duration - 0.1f, ease: easeAlpha))
            .Group(Tween.Scale(target: panel, endValue: scale, duration: duration, ease: easeScale));

            switch (mainMenuPanelState)
            {
                case MainMenuPanelState.settings:
                    animatedSettings.UpdateUI(status);
                    break;
                case MainMenuPanelState.credits:
                    animatedCredits.UpdateUI(status);
                    break;
            }
        }

        private void ShowContent(RectTransform content)
        {
            settingsContent.gameObject.SetActive(false);
            creditsContent.gameObject.SetActive(false);

            content.gameObject.SetActive(true);
        }

        private void OnClosePanel()
        {
            IsOpeningPanel(false);
            mainMenuPanelState = MainMenuPanelState.main;
        }

        private void OnStartBtnClicked() { TransitionManager.Instance.LoadScene("MainWorld", false); }
        private void OnSettingsBtnClicked()
        {
            mainMenuPanelState = MainMenuPanelState.settings;
            IsOpeningPanel(true);
            ShowContent(settingsContent);
        }
        private void OnCreditsBtnClicked()
        {
            mainMenuPanelState = MainMenuPanelState.credits;
            IsOpeningPanel(true);
            ShowContent(creditsContent);
        }
        private void OnExitBtnClicked()
        {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #endif
            Application.Quit();
        }
    }
}
