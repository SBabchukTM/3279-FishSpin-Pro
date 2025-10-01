using System;
using System.Collections.Generic;
using System.Linq;
using AssetsProvider;
using Configs;
using Services;
using UI.Popups;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace UI.Screens
{
    public class CalendardScreen : SelectableScreen
    {
        [SerializeField] private MonthSelectButton _julyButton;
        [SerializeField] private MonthSelectButton _augustButton;
        [SerializeField] private MonthSelectButton _septemberButton;

        [FormerlySerializedAs("_activeDay")] [SerializeField] private Sprite _uncheckedDay;
        [SerializeField] private Sprite _lockedDay;
        [SerializeField] private Sprite _checkedDay;
        
        [SerializeField] private List<CalendarDayButton> _dayButtons;
        
        private MonthSelectButton _activeButton;
        
        [Inject]
        private UserDataManager _userDataManager;
        
        [Inject]
        private UIPrefabFactory _uiPrefabFactory;
        
        [Inject]
        private ConfigsProvider _configsProvider;
        
        private void Start()
        {
            _julyButton.OnClick += () => ChangeMonth(_julyButton);
            _augustButton.OnClick += () => ChangeMonth(_augustButton);
            _septemberButton.OnClick += () => ChangeMonth(_septemberButton);
            
            foreach (var dayButton in _dayButtons)
                dayButton.OnClicked += OnDayClicked;
            
            ChangeMonth(_augustButton);
        }

        private void OnDayClicked(CalendarDayButton button)
        {
            DateTime clickedDate = button.GetDate();
            
            if(clickedDate.Date > DateTime.Now.Date)
                return;
            
            var records = _userDataManager.GetData().UserRecordsData.Records;
            
            var existingRecord = records.FirstOrDefault(r =>
                r.year == clickedDate.Year &&
                r.month == clickedDate.Month &&
                r.day == clickedDate.Day);
            
            if (!records.Contains(existingRecord))
            {
                var popup = _uiPrefabFactory.CreatePopup<CalendarRecordPopup>();
                popup.OnRecorded += (name, location, fishId, time, weight) =>
                {
                    var newRecord = new UserFishRecord(clickedDate.Year, clickedDate.Month, clickedDate.Day)
                    {
                        Name = name,
                        Location = location,
                        FishId = fishId,
                        Time = time,
                        Weight = weight
                    };
                    
                    records.Add(newRecord);
                    popup.DestroyPopup();
                    button.SetImage(_checkedDay);
                };
            }
            else
            {
                var popup = _uiPrefabFactory.CreatePopup<RecordViewPopup>();
                popup.SetData(existingRecord, _configsProvider.Get<FishesConfig>().Fishes[existingRecord.FishId].Name);
            }
        }

        private void ChangeMonth(MonthSelectButton month)
        {
            if(month == _activeButton)
                return;
            
            _activeButton?.SetActive(false);
            _activeButton = month;
            _activeButton.SetActive(true);
            UpdateCalendar();
        }

        private void UpdateCalendar()
        {
            int year = 2025;
            int month = 0;

            if (_activeButton == _julyButton) month = 7;
            else if (_activeButton == _augustButton) month = 8;
            else if (_activeButton == _septemberButton) month = 9;

            int daysInMonth = DateTime.DaysInMonth(year, month);
            DateTime firstDayOfMonth = new DateTime(year, month, 1);
            DayOfWeek firstDayOfWeek = firstDayOfMonth.DayOfWeek;

            int startOffset = ((int)firstDayOfWeek + 6) % 7;

            DateTime today = DateTime.Now;

            var records = _userDataManager.GetData().UserRecordsData.Records;
            
            for (int i = 0; i < _dayButtons.Count; i++)
            {
                CalendarDayButton dayButton = _dayButtons[i];

                int dayNumber = i - startOffset + 1;

                if (dayNumber < 1 || dayNumber > daysInMonth)
                {
                    dayButton.SetActive(false);
                    continue;
                }

                dayButton.SetActive(true);
                dayButton.SetDay(dayNumber);

                DateTime buttonDate = new DateTime(year, month, dayNumber);
                bool futureDay = buttonDate.Date > today.Date;

                dayButton.SetDate(buttonDate);
                dayButton.SetImage(futureDay ? _lockedDay : _uncheckedDay);
                
                bool isRecorded = records.Any(r =>
                    r.year == buttonDate.Year &&
                    r.month == buttonDate.Month &&
                    r.day == buttonDate.Day);

                if (isRecorded)
                {
                    dayButton.SetImage(_checkedDay);
                }
            }
        }
    }
}