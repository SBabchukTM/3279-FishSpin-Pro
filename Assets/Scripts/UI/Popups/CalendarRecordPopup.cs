using System;
using AssetsProvider;
using Configs;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Popups
{
    public class CalendarRecordPopup : BasePopup
    {
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _recordButton;
        [SerializeField] private TMP_InputField _nameField;
        [SerializeField] private TMP_InputField _locationField;
        [SerializeField] private TMP_InputField _weightField;
        [SerializeField] private Button _typeButton;
        [SerializeField] private TextMeshProUGUI _typeText;

        [SerializeField] private DaySelectButton _dawnButton;
        [SerializeField] private DaySelectButton _dayButton;
        [SerializeField] private DaySelectButton _eveningButton;
        [SerializeField] private DaySelectButton _nightButton;

        [SerializeField] private TextMeshProUGUI _errorText;
        
        private int _fishId = 0;
        private string _dayTime;
        
        private DaySelectButton _activeButton;
        
        private Sequence _sequence;
        
        [Inject]
        private UIPrefabFactory _uiPrefabFactory;
        
        [Inject]
        private ConfigsProvider _configsProvider;
        
        public event Action<string, string, int, string, string> OnRecorded;

        private void Awake()
        {
            ChangeDay(_dawnButton);
            
            _closeButton.onClick.AddListener(DestroyPopup);
            _recordButton.onClick.AddListener(() =>
            {
                if (ValidateFish())
                    OnRecorded?.Invoke(_nameField.text, _locationField.text, _fishId, _dayTime, _weightField.text);
            });
            
            _dawnButton.OnClick += () => ChangeDay(_dawnButton);
            _dayButton.OnClick += () => ChangeDay(_dayButton);
            _eveningButton.OnClick += () => ChangeDay(_eveningButton);
            _nightButton.OnClick += () => ChangeDay(_nightButton);
            
            _typeButton.onClick.AddListener(() =>
            {
                var popup = _uiPrefabFactory.CreatePopup<FishTypeSelectPopup>();
                popup.OnSelect += index =>
                {
                    _fishId = index;
                    _typeText.text = _configsProvider.Get<FishesConfig>().Fishes[index].Name;
                };
            });
        }

        private bool ValidateFish()
        {
            if (_nameField.text.Length == 0)
            {
                ShowError("Enter a record name!");
                return false;
            }
            
            if (_locationField.text.Length == 0)
            {
                ShowError("Enter a location name!");
                return false;
            }

            var text = _weightField.text;
            text = text.Replace('.', ',');
            
            if (!float.TryParse(text, out var result))
            {
                ShowError("Enter a valid weight!");
                return false;
            }

            if (result is <= 0 or > 999)
            {
                ShowError("Enter a valid weight!");
                return false;
            }
            
            return true;
        }

        private void ChangeDay(DaySelectButton month)
        {
            if(month == _activeButton)
                return;
            
            _activeButton?.SetActive(false);
            _activeButton = month;
            _activeButton.SetActive(true);
            _dayTime = _activeButton.DayName;
        }

        private void ShowError(string error)
        {
            _sequence?.Kill();
            _sequence = DOTween.Sequence();
            
            var color = _errorText.color;
            color.a = 0;
            _errorText.color = color;
            
            _errorText.text = error;
            _sequence.Append(_errorText.DOFade(1, 0.2f));
            _sequence.AppendInterval(0.5f);
            _sequence.Append(_errorText.DOFade(0, 0.2f));
        }
    }
}