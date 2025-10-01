using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Popups
{
    public class TermsOfUsePopup : BasePopup
    {
        [SerializeField] private Button _button;

        private void Awake()
        {
            _button.onClick.AddListener(DestroyPopup);
        }
    }
}