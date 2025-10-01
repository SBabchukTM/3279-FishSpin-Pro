using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Screens
{
    public class OnboardingScreen : BaseScreen
    {
        [SerializeField] private HorizontalPageSlider _horizontalPageSlider;
        [SerializeField] private Button _startButton;

        public event Action OnStartButtonPressed;
        
        private void Awake()
        {
            _horizontalPageSlider.PageChanged += ShowStartButton;
        }

        private void ShowStartButton(int obj)
        {
            if (obj == _horizontalPageSlider.TotalPages - 1)
            {
                _startButton.gameObject.SetActive(true);
                _startButton.onClick.AddListener(() => OnStartButtonPressed?.Invoke());
            }
        }
    }
}