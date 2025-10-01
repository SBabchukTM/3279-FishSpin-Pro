using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Services;
using UI.Popups;
using UI.Screens;
using UnityEngine;
using Zenject;

namespace UI
{
    public class UIPrefabFactory : MonoBehaviour
    {
        [SerializeField] private float _fadeTime = 0.25f;

        [SerializeField] private RectTransform _canvas;
        [SerializeField] private CanvasGroup _fadeCanvas;

        [SerializeField] private BaseScreen[] _screens;
        [SerializeField] private BasePopup[] _popups;
    
        private readonly Dictionary<Type, BaseScreen> _gameScreens = new();
        private readonly Dictionary<Type, BasePopup> _gamePopups = new();

        private readonly List<BaseScreen> _openedScreens = new();

        [Inject] 
        private DIFactory _factory;

        private ScreenManager _screenManager;
        private PopupManager _popupManager;
        private UIFader _uiFader;

        private void Awake()
        {
            // Initialize managers
            _screenManager = new ScreenManager(_screens, _canvas);
            _popupManager = new PopupManager(_popups, _canvas);
            _uiFader = new UIFader(_fadeCanvas, _fadeTime);
        
            // Register screens and popups
            foreach (var screen in _screens)
                _gameScreens.Add(screen.GetType(), screen);

            foreach (var popup in _popups)
                _gamePopups.Add(popup.GetType(), popup);
        }

        public async UniTask<T> CreateScreen<T>() where T : BaseScreen
        {
            return await _screenManager.CreateScreen<T>(_gameScreens, _openedScreens, _factory, _uiFader);
        }

        public T CreatePopup<T>() where T : BasePopup
        {
            return _popupManager.CreatePopup<T>(_gamePopups, _canvas, _factory);
        }

        public async UniTask HideScreen<T>() where T : BaseScreen
        {
            await _screenManager.HideScreen<T>(_openedScreens, _uiFader);
        }
    }
}