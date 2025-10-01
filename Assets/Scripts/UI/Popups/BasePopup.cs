using System;
using UnityEngine;
using Zenject;

namespace UI.Popups
{
    public abstract class BasePopup : MonoBehaviour
    {
        [Inject]
        protected SoundPlayer _soundPlayer;
        
        private void Start()
        {
            _soundPlayer.PlayPopupSound();
        }

        public void DestroyPopup()
        {
            GameObject.Destroy(gameObject);
            _soundPlayer.PlayButtonSound();
        }
    }
}