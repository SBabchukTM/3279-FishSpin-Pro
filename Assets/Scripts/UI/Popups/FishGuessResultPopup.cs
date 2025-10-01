using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Popups
{
    public class FishGuessResultPopup : BasePopup
    {
        [SerializeField] private Button _button;
        [SerializeField] private Image _image;
        
        private void Awake()
        {
            Time.timeScale = 0;
            _button.onClick.AddListener(DestroyPopup);
        }
        
        public void SetFish(Sprite sprite) => _image.sprite = sprite;

        private void OnDestroy()
        {
            Time.timeScale = 1;
        }
    }
}