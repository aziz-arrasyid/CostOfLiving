using PrimeTween;
using UnityEngine;

namespace Main.Menu
{
    public class AnimatedCredits : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private RectTransform character;
        [SerializeField] private RectTransform logo;
        [SerializeField] private RectTransform mainMenuBtn;

        private Sequence anim;

        public void UpdateUI(bool isOpen)
        {
            if (anim.isAlive) anim.Stop();

            anim = Sequence.Create();

            CanvasGroup cgCharacter = character.GetComponent<CanvasGroup>();
            CanvasGroup cgMainMenuBtn = mainMenuBtn.GetComponent<CanvasGroup>();

            float alpha = isOpen ? 0f : 1f;
            float duration = 0.4f;
            Vector2 logoPositionX = isOpen ? new Vector2(-19f, -333f) : new Vector2(561f, -333f);
            Vector2 logoScale = isOpen ? Vector2.one / 10 * 8 : Vector2.one;
            Ease easeAlpha = isOpen ? Ease.InSine : Ease.OutSine;
            Ease easePosition = Ease.InOutSine;
            Ease easeScale = isOpen ? Ease.InSine : Ease.OutSine;

            anim
            .Group(Tween.Alpha(target: cgCharacter, endValue: alpha, duration: duration, ease: easeAlpha))
            .Group(Tween.Alpha(target: cgMainMenuBtn, endValue: alpha, duration: duration, ease: easeAlpha))
            .Group(Tween.UIAnchoredPosition(target: logo, endValue: logoPositionX, duration: duration, ease: easePosition))
            .Group(Tween.Scale(target: logo, endValue: logoScale, duration: duration, ease: easeScale));
        }
    }
}
