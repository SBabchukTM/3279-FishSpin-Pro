using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace UI
{
    public class UIFader
    {
        private readonly CanvasGroup _fadeCanvas;
        private readonly float _fadeTime;

        public UIFader(CanvasGroup fadeCanvas, float fadeTime)
        {
            _fadeCanvas = fadeCanvas;
            _fadeTime = fadeTime;
        }

        public async UniTask FadeIn()
        {
            _fadeCanvas.alpha = 0;
            _fadeCanvas.interactable = true;
            _fadeCanvas.blocksRaycasts = true;

            _fadeCanvas.DOFade(1, _fadeTime);
            await UniTask.WaitForSeconds(_fadeTime);

            _fadeCanvas.interactable = false;
            _fadeCanvas.blocksRaycasts = false;
        }

        public async UniTask FadeOut()
        {
            _fadeCanvas.alpha = 1;
            _fadeCanvas.interactable = true;
            _fadeCanvas.blocksRaycasts = true;

            _fadeCanvas.DOFade(0, _fadeTime);
            await UniTask.WaitForSeconds(_fadeTime);

            _fadeCanvas.interactable = false;
            _fadeCanvas.blocksRaycasts = false;
        }
    }
}