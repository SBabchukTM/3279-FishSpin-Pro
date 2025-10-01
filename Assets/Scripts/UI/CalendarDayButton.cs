using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class CalendarDayButton : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private Button _button;
        [SerializeField] private GameObject _content;
        [SerializeField] private TextMeshProUGUI _dayText;
    
        public event Action<CalendarDayButton> OnClicked;
    
        private DateTime _date;

        private void Awake()
        {
            _button.onClick.AddListener(() => OnClicked?.Invoke(this));
        }
    
        public void SetImage(Sprite sprite) => _image.sprite = sprite;
        public void SetActive(bool active) => _content.SetActive(active);
        public void SetDay(int day) => _dayText.text = day.ToString();
    
        public void SetDate(DateTime date) => _date = date;
        public DateTime GetDate() => _date;
    }
}
