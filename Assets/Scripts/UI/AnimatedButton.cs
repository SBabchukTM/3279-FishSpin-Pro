using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    [RequireComponent(typeof(Button))]
    public class AnimatedButton : MonoBehaviour
    {
        [SerializeField] private bool _actionSound;
    
        [Inject]
        private SoundPlayer _soundPlayer;
    
        private Sequence _sequence;
    
        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(ProcessClick);
        }

        private void ProcessClick()
        {
            if(_actionSound)
                _soundPlayer.PlayActionSound();
            else
                _soundPlayer.PlayButtonSound();
        
            _sequence?.Kill();
            _sequence = DOTween.Sequence();

            _sequence.Append(transform.DOScale(Vector3.one * 1.05f, 0.2f));
            _sequence.Append(transform.DOScale(Vector3.one, 0.3f));
        }
    }
}
