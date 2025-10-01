using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace UI
{
    public class HorizontalPageSlider  : MonoBehaviour
    {
        [Header("Swipe Configuration"), Space]
        [SerializeField] private bool _allowSwipeLeft = true;
        [SerializeField] private bool _allowSwipeRight = true;
        [SerializeField] private float _swipeSensitivity = 50f;

        [Header("Layout References"), Space]
        [SerializeField] private RectTransform _contentHolder;
        [SerializeField] private List<RectTransform> _pageList;
        [SerializeField] private float _transitionDuration = 0.5f;

        private int _currentPage = 0;
        private float _pageWidth = 0f;
        private Vector2 _initialTouchPosition;
        private bool _isTouching = false;

        public int TotalPages => _pageList.Count;
        public event Action<int> PageChanged;
        
        private void Start()
        {
            StartCoroutine(SetupLayoutAfterFrame());
        }

        private void Update()
        {
            HandleTouchSwipe();
        }

        private void HandleTouchSwipe()
        {
            if (!_allowSwipeLeft && !_allowSwipeRight)
                return;

            if (Input.touchCount == 0)
                return;

            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                _initialTouchPosition = touch.position;
                _isTouching = true;
            }
            else if (touch.phase == TouchPhase.Ended && _isTouching)
            {
                Vector2 swipeDelta = touch.position - _initialTouchPosition;
                EvaluateSwipeDirection(swipeDelta);
                _isTouching = false;
            }
        }

        private void EvaluateSwipeDirection(Vector2 delta)
        {
            if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y) && Mathf.Abs(delta.x) > _swipeSensitivity)
            {
                if (delta.x > 0 && _allowSwipeRight && _currentPage > 0)
                {
                    ChangePage(-1);
                }
                else if (delta.x < 0 && _allowSwipeLeft && _currentPage < _pageList.Count - 1)
                {
                    ChangePage(1);
                }
            }
        }

        private IEnumerator SetupLayoutAfterFrame()
        {
            yield return new WaitForEndOfFrame();
            Canvas.ForceUpdateCanvases();

            RectTransform viewport = (RectTransform)_contentHolder.parent;
            _pageWidth = viewport.rect.width;

            _contentHolder.anchorMin = new Vector2(0, 0);
            _contentHolder.anchorMax = new Vector2(0, 1);
            _contentHolder.pivot = new Vector2(0, 0.5f);

            for (int i = 0; i < _pageList.Count; i++)
            {
                RectTransform page = _pageList[i];
                page.anchorMin = new Vector2(0, 0);
                page.anchorMax = new Vector2(0, 1);
                page.pivot = new Vector2(0, 0.5f);

                page.sizeDelta = new Vector2(_pageWidth, 0);
                page.anchoredPosition = new Vector2(i * _pageWidth, 0);
            }

            _contentHolder.sizeDelta = new Vector2(_pageWidth * _pageList.Count, 0);
            _contentHolder.anchoredPosition = Vector2.zero;
        }

        private void ChangePage(int direction)
        {
            _currentPage += direction;
            _currentPage = Mathf.Clamp(_currentPage, 0, _pageList.Count - 1);
            AnimateToPage(_currentPage);
        }

        private void AnimateToPage(int index)
        {
            float targetPositionX = -index * _pageWidth;
            _contentHolder.DOAnchorPosX(targetPositionX, _transitionDuration)
                .SetEase(Ease.InOutCubic)
                .SetLink(gameObject)
                .OnComplete(() =>
                {
                    PageChanged?.Invoke(_currentPage);
                });
        }
    }
}
