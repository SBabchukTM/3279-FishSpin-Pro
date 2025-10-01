using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Screens
{
    public class LoadingScreen : BaseScreen
    {
        [SerializeField] private Slider _loadSlider;
        [SerializeField] private float _loadTime;
        
        public async UniTask Load()
        {
            _loadSlider.value = 0;
            _loadSlider.DOValue(1, _loadTime).SetEase(Ease.OutCubic);
            await UniTask.WaitForSeconds(_loadTime);
        }
    }
}