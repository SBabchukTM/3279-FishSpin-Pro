using System;
using UI.Popups;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Screens
{
    public class MainScreen : BaseScreen
    {
        [SerializeField] private EntryScreen _entryScreen;
        [SerializeField] private GuessGameStartScreen _guessGameStartScreen;
        [SerializeField] private CalendardScreen _calendardScreen;
        [SerializeField] private StatsScreen _statsScreen;
        [SerializeField] private LibraryScreen _libraryScreen;

        [SerializeField] private Button _setButton;
        [SerializeField] private Button _profButton;
        [SerializeField] private Button _quizButton;
        
        private SelectableScreen _currentScreen;

        public event Action OnSettingsPressed;
        public event Action OnProfilePressed;
        public event Action OnQuizPressed;
        
        private void Awake()
        {
            _currentScreen = _entryScreen;
            
            _guessGameStartScreen.FooterButton.onClick.AddListener(() => ChangeScreen(_guessGameStartScreen));
            _calendardScreen.FooterButton.onClick.AddListener(() => ChangeScreen(_calendardScreen));
            _statsScreen.FooterButton.onClick.AddListener(() => ChangeScreen(_statsScreen));
            _libraryScreen.FooterButton.onClick.AddListener(() => ChangeScreen(_libraryScreen));
            
            _guessGameStartScreen.OnBackPressed += () => ChangeScreen(_entryScreen);
            _calendardScreen.OnBackPressed += () => ChangeScreen(_entryScreen);
            _statsScreen.OnBackPressed += () => ChangeScreen(_entryScreen);
            _libraryScreen.OnBackPressed += () => ChangeScreen(_entryScreen);
            
            _setButton.onClick.AddListener(() => OnSettingsPressed?.Invoke());
            _profButton.onClick.AddListener(() => OnProfilePressed?.Invoke());
            _quizButton.onClick.AddListener(() => OnQuizPressed?.Invoke());
        }

        private void ChangeScreen(SelectableScreen newScreen)
        {
            if(_currentScreen != null)
                _currentScreen.HideScreen();
            
            _currentScreen = newScreen;
            _currentScreen.ShowScreen();
        }
    }
}