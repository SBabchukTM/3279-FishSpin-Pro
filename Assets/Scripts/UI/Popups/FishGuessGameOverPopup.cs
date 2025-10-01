using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Popups
{
    public class FishGuessGameOverPopup : BasePopup
    {
        [SerializeField] private TextMeshProUGUI _correctText;
        [SerializeField] private TextMeshProUGUI _wrongText;
        [SerializeField] private Button _button;

        public event Action OnClose;
        
        private void Awake()
        {
            _button.onClick.AddListener(() =>
            {
                DestroyPopup();
                OnClose?.Invoke();
            });
        }

        public void SetData(int correct, int wrong)
        {
            _correctText.text = correct.ToString();
            _wrongText.text = wrong.ToString();
        }
    }
}