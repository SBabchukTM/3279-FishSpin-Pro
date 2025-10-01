using Services;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Popups
{
    public class SettingsPopup : BasePopup
    {
        [SerializeField] private Slider _soundSlider;
        [SerializeField] private Slider _musicSlider;

        [SerializeField] private Button _ppButton;
        [SerializeField] private Button _touButton;

        [SerializeField] private Button _closeButton;

        [Inject]
        private UserDataManager _userDataManager;
        
        [Inject]
        private UIPrefabFactory _uiPrefabFactory;
        
        [Inject]
        private MusicPlayer _musicPlayer;
        
        private void Awake()
        {
            _soundSlider.value = _userDataManager.GetData().GameSettingsData.SoundVolume;
            _musicSlider.value = _userDataManager.GetData().GameSettingsData.MusicVolume;
            
            _soundSlider.onValueChanged.AddListener((value) =>
            {
                _userDataManager.GetData().GameSettingsData.SoundVolume = value;
                _soundPlayer.SetVolume(value);
            });
            
            _musicSlider.onValueChanged.AddListener((value) =>
            {
                _userDataManager.GetData().GameSettingsData.MusicVolume = value;
                _musicPlayer.SetVolume(value);
            });
            
            _ppButton.onClick.AddListener(() => _uiPrefabFactory.CreatePopup<PrivacyPolicyPopup>());
            _touButton.onClick.AddListener(() => _uiPrefabFactory.CreatePopup<TermsOfUsePopup>());
            
            _closeButton.onClick.AddListener(DestroyPopup);
        }
    }
}