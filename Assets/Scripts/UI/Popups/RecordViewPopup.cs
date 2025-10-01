using System;
using Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Popups
{
    public class RecordViewPopup : BasePopup
    {
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _locationText;
        [SerializeField] private TextMeshProUGUI _typeText;
        [SerializeField] private TextMeshProUGUI _timeText;
        [SerializeField] private TextMeshProUGUI _weightText;
        [SerializeField] private Button _button;

        private void Awake()
        {
            _button.onClick.AddListener(DestroyPopup);
        }

        public void SetData(UserFishRecord record, string fishName)
        {
            _nameText.text = record.Name;
            _locationText.text = record.Location;
            _typeText.text = fishName;
            _timeText.text = record.Time;
            _weightText.text = record.Weight;
        }
    }
}