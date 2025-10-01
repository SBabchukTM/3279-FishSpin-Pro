using System;
using System.Linq;
using AssetsProvider;
using Configs;
using Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Popups
{
    public class ProfilePopup : BasePopup
    {
        [SerializeField] private Button _button;

        [SerializeField] private TextMeshProUGUI _favCatchText;
        [SerializeField] private TextMeshProUGUI _biggestCatchText;
        
        [Inject]
        private UserDataManager _userDataManager;
        
        [Inject]
        private ConfigsProvider _configsProvider;
        
        private void Awake()
        {
            _button.onClick.AddListener(DestroyPopup);
            SetData();
        }

        private void SetData()
        {
            var data = _userDataManager.GetData().UserRecordsData.Records;

            if (data == null || data.Count == 0)
            {
                _favCatchText.text = "No Data";
                _biggestCatchText.text = "No Data";
                return;
            }

            // Most frequent fish name
            int mostFrequentFish = data
                .GroupBy(record => record.FishId)
                .OrderByDescending(group => group.Count())
                .First()
                .Key;

            // Fish with the highest weight
            UserFishRecord heaviestFish = data
                .OrderByDescending(record =>
                {
                    float.TryParse(record.Weight, out float parsedWeight);
                    return parsedWeight;
                })
                .First();
            
            _favCatchText.text = _configsProvider.Get<FishesConfig>().Fishes[mostFrequentFish].Name;
            _biggestCatchText.text = _configsProvider.Get<FishesConfig>().Fishes[heaviestFish.FishId].Name + "\n" + heaviestFish.Weight +" kg";
        }
    }
}