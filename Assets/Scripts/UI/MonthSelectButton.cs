using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MonthSelectButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private TextMeshProUGUI _text;
        
        public event Action OnClick;
        
        private Sequence _sequence;

        private void Awake()
        {
            _button.onClick.AddListener(() => OnClick?.Invoke());
        }

        public void SetActive(bool active)
        {
            _sequence?.Kill();
            _sequence = DOTween.Sequence();
            
            _sequence.Append(_button.image.DOFade(active ? 1 : 0, 0.5f));
            _sequence.Join(_text.DOColor(active ? Color.blue : Color.white, 0.5f));
        }
    }
}