using System;
using Configs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Popups
{
    public class FishDataPopup : BasePopup
    {
        [SerializeField] private Image _image;
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _tempText;
        [SerializeField] private TextMeshProUGUI _foodText;
        [SerializeField] private TextMeshProUGUI _factText;
        [SerializeField] private TextMeshProUGUI _timeText;

        [SerializeField] private Button _button;

        private void Awake()
        {
            _button.onClick.AddListener(DestroyPopup);
        }

        public void SetData(FishData fishData)
        {
            _image.sprite = fishData.Sprite;
            _nameText.text = fishData.Name;
            _tempText.text = fishData.Temperature;
            _foodText.text = fishData.FavFood;
            _factText.text = fishData.FunFact;
            _timeText.text = fishData.BestTime;
        }
    }
}