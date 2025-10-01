using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class QuizButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private TextMeshProUGUI _choiceText;
    
        public event Action OnClicked;
    
        public void SetText(string text) => _choiceText.text = text;

        private void Awake()
        {
            _button.onClick.AddListener(() => OnClicked?.Invoke());
        }
    
        public void SetColor(Color color) => _button.image.color = color;
    }
}
