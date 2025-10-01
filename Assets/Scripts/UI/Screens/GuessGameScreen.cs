using System;
using System.Collections.Generic;
using System.Linq;
using AssetsProvider;
using Configs;
using DG.Tweening;
using Services;
using TMPro;
using UI.Popups;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Screens
{
    public class GuessGameScreen : BaseScreen
    {
        [SerializeField] private Button _backButton;

        [SerializeField] private TextMeshProUGUI _questionCountText;
        
        [SerializeField] private Color _correctColor;
        [SerializeField] private Color _wrongColor;

        [SerializeField] private List<QuizButton> _choiceButtons;

        [SerializeField] private Image _fishImage;
        [SerializeField] private CanvasGroup _canvasGroup;
        
        public event Action OnBackPressed;
        
        [Inject]
        private ConfigsProvider _configsProvider;
        
        [Inject]
        private UserDataManager _userDataManager;
        
        [Inject]
        private UIPrefabFactory _uiPrefabFactory;

        private int _questionId = 0;
        
        private List<FishQuizQuestion> _quizQuestions;
        
        private int _correctAnswers = 0;
        private int _wrongAnswers = 0;
        
        private Sequence _sequence;
        
        public void Awake()
        {
            SubscribeEvents();
            
            var generator = new FishQuizGenerator(_configsProvider.Get<FishesConfig>().Fishes);
            _quizQuestions = generator.GenerateQuizQuestions();
            SetupQuestions(_quizQuestions[0]);

            for (int i = 0; i < _choiceButtons.Count; i++)
            {
                int index = i;
                _choiceButtons[i].OnClicked += () => ProcessAnswer(index);
            }
        }
        
        private void SubscribeEvents()
        {
            _backButton.onClick.AddListener(() => OnBackPressed?.Invoke());
        }

        private void SetupQuestions(FishQuizQuestion question)
        {
            _questionCountText.text = (_questionId + 1) + "/" + 15;
            _fishImage.sprite = question.TargetFish.Sprite;
            for (int i = 0; i < _choiceButtons.Count; i++)
            {
                _choiceButtons[i].SetColor(Color.white);
                _choiceButtons[i].SetText(question.Options[i]);
            }
        }

        private void ProcessAnswer(int index)
        {
            _canvasGroup.blocksRaycasts = false;

            var question = _quizQuestions[_questionId];
            var correctAnswer = question.CorrectAnswerIndex;

            if (index == correctAnswer)
            {
                _correctAnswers++;
                _choiceButtons[index].SetColor(_correctColor);
                TryRecordFishGuess();
            }
            else
            {
                _wrongAnswers++;
                _choiceButtons[index].SetColor(_wrongColor);
            }

            _sequence?.Kill();
            _sequence = DOTween.Sequence();
            _sequence.SetLink(gameObject);

            _sequence.AppendInterval(0.5f);
            _sequence.Append(_canvasGroup.DOFade(0, 0.5f));
            _sequence.AppendCallback(() =>
            {
                _questionId++;
        
                if (_questionId < _quizQuestions.Count)
                {
                    SetupQuestions(_quizQuestions[_questionId]);
                }
                else
                {
                    var popup = _uiPrefabFactory.CreatePopup<FishGuessGameOverPopup>();
                    popup.SetData(_correctAnswers, _wrongAnswers);
                    popup.OnClose += () =>
                    {
                        OnBackPressed?.Invoke();
                    };
                }
            });
            
            if(_questionId < _quizQuestions.Count)
            {
                _sequence.Append(_canvasGroup.DOFade(1, 0.5f));
                _sequence.OnComplete(() =>
                {
                    _canvasGroup.blocksRaycasts = true;
                });
            }
        }

        private void TryRecordFishGuess()
        {
            int fishIndex = _questionId;
            var library = _userDataManager.GetData().UserLibraryData.UnlockedFishes;
            if (library.Contains(fishIndex))
                return;
            
            library.Add(fishIndex);
            var popup = _uiPrefabFactory.CreatePopup<FishGuessResultPopup>();
            popup.SetFish(_configsProvider.Get<FishesConfig>().Fishes[fishIndex].Sprite);
        }
    }
    
    public class FishQuizQuestion
    {
        public FishData TargetFish { get; private set; }
        public List<string> Options { get; private set; }
        public int CorrectAnswerIndex { get; private set; }

        public FishQuizQuestion(FishData targetFish, List<string> options, int correctIndex)
        {
            TargetFish = targetFish;
            Options = options;
            CorrectAnswerIndex = correctIndex;
        }
    }

    public class FishQuizGenerator
    {
        private readonly List<FishData> _allFishes;
        private readonly System.Random _random = new System.Random();

        public FishQuizGenerator(List<FishData> fishes)
        {
            _allFishes = fishes;
        }

        public List<FishQuizQuestion> GenerateQuizQuestions()
        {
            var questions = new List<FishQuizQuestion>();

            foreach (var fish in _allFishes)
            {
                var options = GenerateAnswerOptions(fish);
                var correctIndex = options.IndexOf(fish.Name);

                questions.Add(new FishQuizQuestion(fish, options, correctIndex));
            }

            return questions;
        }

        private List<string> GenerateAnswerOptions(FishData correctFish)
        {
            var allOtherFishNames = _allFishes
                .Where(f => f != correctFish)
                .Select(f => f.Name)
                .ToList();

            Shuffle(allOtherFishNames);

            var options = allOtherFishNames
                .Take(3)
                .ToList();

            options.Add(correctFish.Name);
            Shuffle(options);

            return options;
        }

        private void Shuffle<T>(List<T> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int k = _random.Next(i + 1);
                (list[i], list[k]) = (list[k], list[i]);
            }
        }
    }
}