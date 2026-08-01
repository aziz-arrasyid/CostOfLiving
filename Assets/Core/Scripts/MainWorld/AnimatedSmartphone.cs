using UnityEngine;
using UnityEngine.UI;
using PrimeTween;

namespace Main.World
{
    public class AnimatedSmartphone : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private Button phoneIcon;
        [SerializeField] private Button phoneExitIcon;
        [SerializeField] private Button shopAPK;
        [SerializeField] private Button pinjolLegalAPK;
        [SerializeField] private Button pinjolIlegalAPK;
        [SerializeField] private RectTransform shopPanel;
        [SerializeField] private RectTransform pinjolLegalPanel;
        [SerializeField] private RectTransform pinjolIllegalPanel;
        [SerializeField] private RectTransform phone;

        private RectTransform currentAPKOpen;
        private Sequence animPhone;

        private void Awake()
        {
            phoneIcon.onClick.AddListener(() => PhoneStateOpen(true));
            phoneExitIcon.onClick.AddListener(() => PhoneStateOpen(false));
            shopAPK.onClick.AddListener(() => OnAPKClicked(shopPanel));
            pinjolLegalAPK.onClick.AddListener(() => OnAPKClicked(pinjolLegalPanel));
            pinjolIlegalAPK.onClick.AddListener(() => OnAPKClicked(pinjolIllegalPanel));
        }

        private void OnAPKClicked(RectTransform panel)
        {
            if (currentAPKOpen == panel) return;

            float openTargetY = 47f;
            float openAlpha = 1f;
            float openDuration = 0.4f;
            Ease openEase = Ease.OutSine;

            float closeTargetY = -578f;
            float closeAlpha = 0f;
            float closeDuration = 0.4f;
            Ease closeEase = Ease.InSine;

            CanvasGroup cg = panel.GetComponent<CanvasGroup>();
            cg.alpha = 0f;

            Sequence animAPK = Sequence.Create();

            if (currentAPKOpen != null)
            {
                CanvasGroup cgCurrentAPKOpen = currentAPKOpen.GetComponent<CanvasGroup>();
                animAPK
                .Group(Tween.Alpha(target: cgCurrentAPKOpen, endValue: closeAlpha, duration: closeDuration - 0.1f, ease: closeEase))
                .Group(Tween.UIAnchoredPositionY(target: currentAPKOpen, endValue: closeTargetY, duration: closeDuration, ease: closeEase));
            }

            animAPK
            .Chain(Tween.Alpha(target: cg, endValue: openAlpha, duration: openDuration - 0.1f, ease: openEase))
            .Group(Tween.UIAnchoredPositionY(target: panel, endValue: openTargetY, duration: openDuration, ease: openEase));

            currentAPKOpen = panel;

        }

        private void CloeAPKWhilePhoneClosed()
        {
            Sequence animAPK = Sequence.Create();

            float closeTargetY = -610f;
            float closeAlpha = 0f;
            float closeDuration = 0.4f;
            Ease closeEase = Ease.InSine;

            CanvasGroup cgCurrentAPKOpen = currentAPKOpen.GetComponent<CanvasGroup>();
            animAPK
            .Group(Tween.Alpha(target: cgCurrentAPKOpen, endValue: closeAlpha, duration: closeDuration - 0.1f, ease: closeEase))
            .Group(Tween.UIAnchoredPositionY(target: currentAPKOpen, endValue: closeTargetY, duration: closeDuration, ease: closeEase))
            .OnComplete(target: ShopManager.Instance, target => target.ResetItemSelected());

            currentAPKOpen = null;
        }

        private void PhoneStateOpen(bool status)
        {
            if (animPhone.isAlive) animPhone.Stop();
            animPhone = Sequence.Create();

            CanvasGroup cgPhoneIcon = phoneIcon.GetComponent<CanvasGroup>();
            CanvasGroup cgPhone = phone.GetComponent<CanvasGroup>();

            cgPhone.alpha = status ? 0f : 1f;

            float cgPhoneIconDuration = status ? 0.1f : 0.5f;
            float cgPhoneIconEndValue = status ? 0f : 1f;
            Ease phoneIconEase = status ? Ease.InSine : Ease.OutSine;

            float phoneTargetX = status ? -192f : 443f;
            float phoneDuration = 0.4f;
            float cgPhoneEndValue = status ? 1f : 0f;
            Ease phoneEase = status ? Ease.OutSine : Ease.InSine;

            float phoneIconDelay = status ? 0f : phoneDuration / 2;

            if (currentAPKOpen != null)
            {
                CloeAPKWhilePhoneClosed();
            }

            animPhone
            // Animasi Phone Icon
            .Group(Tween.Alpha(target: cgPhoneIcon, endValue: cgPhoneIconEndValue, duration: cgPhoneIconDuration, ease: phoneIconEase, startDelay: phoneIconDelay))
            // Animasi Phone Icon
            // Animasi Phone Body
            .Group(Tween.UIAnchoredPositionX(target: phone, endValue: phoneTargetX, duration: phoneDuration, ease: phoneEase))
            .Group(Tween.Alpha(target: cgPhone, endValue: cgPhoneEndValue, duration: phoneDuration - 0.1f, ease: phoneEase));
            //Animasi Phone Body
        }
    }
}
