using DG.Tweening;
using UnityEngine;

namespace UI
{
    public class PageIndicatorAnimator : MonoBehaviour
    {
        private static readonly Vector3 EnlargedScale = Vector3.one * 1.3f;
        private const float AnimationDuration = 0.3f;

        [SerializeField] private Transform[] _indicatorDots;
        [SerializeField] private HorizontalPageSlider _pageSlider;

        private int _lastSelectedIndex = 0;

        private void Awake()
        {
            _pageSlider.PageChanged += OnPageChanged;

            _lastSelectedIndex = 0;
            AnimateEnlarge(_lastSelectedIndex);
        }

        private void OnDestroy()
        {
            _pageSlider.PageChanged -= OnPageChanged;
        }

        private void OnPageChanged(int currentIndex)
        {
            AnimateEnlarge(currentIndex);
            AnimateShrink(_lastSelectedIndex);
            _lastSelectedIndex = currentIndex;
        }

        private void AnimateEnlarge(int index)
        {
            _indicatorDots[index]
                .DOScale(EnlargedScale, AnimationDuration)
                .SetLink(gameObject);
        }

        private void AnimateShrink(int index)
        {
            _indicatorDots[index]
                .DOScale(Vector3.one, AnimationDuration)
                .SetLink(gameObject);
        }
    }
}
