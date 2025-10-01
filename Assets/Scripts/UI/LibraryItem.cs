using System;
using Configs;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LibraryItem : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private GameObject _lock;
        [SerializeField] private Button _button;

        public event Action<FishData> OnClicked;
        
        private FishData _fishData;

        private void Awake()
        {
            _button.onClick.AddListener(() => OnClicked?.Invoke(_fishData));
        }

        public void Initialize(FishData fishData, bool locked)
        {
            _fishData = fishData;
            _button.interactable = !locked;
            
            _image.sprite = fishData.Sprite;
            _image.color = !locked ? Color.white : Color.black;
            
            _lock.SetActive(locked);
        }
    }
}