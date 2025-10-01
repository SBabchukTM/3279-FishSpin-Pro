using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Screens
{
    public class SelectableScreen : MonoBehaviour
    {
        private const float FadeTime = 0.75f;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _footerButton;
        
        private Sequence _sequence;
        
        public event Action OnBackPressed;
        public Button FooterButton => _footerButton;
        
        [Inject]
        private SoundPlayer _soundPlayer;

        private void Awake()
        {
            if(_backButton)
                _backButton.onClick.AddListener(() => OnBackPressed?.Invoke());
        }

        public void ShowScreen()
        {
            _sequence?.Kill();
            _sequence = DOTween.Sequence();
            
            _sequence.Append(_canvasGroup.DOFade(1, FadeTime));
            _sequence.OnComplete(() =>
            {
                _canvasGroup.blocksRaycasts = true;
            });
            
            if (_footerButton)
            {
                _footerButton.transform.DOKill();
                _footerButton.transform.DOScale(Vector3.one * 1.05f, 0.25f);
                _footerButton.enabled = false;
                _soundPlayer.PlayActionSound();
            }

            ProcessScreenOpen();
        }

        public void HideScreen()
        {
            _sequence?.Kill();
            _sequence = DOTween.Sequence();
            
            _canvasGroup.blocksRaycasts = false;
            
            _sequence.Append(_canvasGroup.DOFade(0, FadeTime));

            if (_footerButton)
            {
                _footerButton.transform.DOKill();
                _footerButton.transform.DOScale(Vector3.one, 0.25f);
                _footerButton.enabled = true;
            }
            ProcessScreenClose();
        }

        public virtual void ProcessScreenOpen()
        {
            
        }

        public virtual void ProcessScreenClose()
        {
            
        }
    }
}