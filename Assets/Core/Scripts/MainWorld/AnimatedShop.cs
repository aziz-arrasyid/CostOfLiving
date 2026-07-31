using UnityEngine;
using PrimeTween;

namespace Main.World
{
    public class AnimatedShop : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private RectTransform previewContent;
        private CanvasGroup cgPreviewContent;
        private bool isFirstView;

        private void Start()
        {
            cgPreviewContent = previewContent.GetComponent<CanvasGroup>();
            cgPreviewContent.alpha = 0f;
        }

        public void PreviewItem()
        {
            if (isFirstView) return;

            cgPreviewContent.alpha = 0f;

            float duration = 0.4f;

            Sequence animPreview = Sequence.Create();

            animPreview
            .Group(Tween.Alpha(target: cgPreviewContent, endValue: 1f, duration: duration - 0.1f, ease: Ease.OutSine))
            .Group(Tween.UIAnchoredPositionX(target: previewContent, endValue: -1610f, duration: duration, ease: Ease.OutSine));

            isFirstView = true;
        }

        public void ResetPreviewItem()
        {
            isFirstView = false;
            cgPreviewContent.alpha = 0f;

            Vector2 newPos = previewContent.anchoredPosition;
            newPos.x = -1163f;
            previewContent.anchoredPosition = newPos;
        }
    }
}
