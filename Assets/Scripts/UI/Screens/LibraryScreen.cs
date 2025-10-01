using System;
using System.Collections.Generic;
using AssetsProvider;
using Configs;
using Services;
using UI.Popups;
using UnityEngine;
using Zenject;

namespace UI.Screens
{
    public class LibraryScreen : SelectableScreen
    {
        [SerializeField] private List<LibraryItem> _list;
        
        [Inject]
        private ConfigsProvider _configsProvider;
        
        [Inject]
        private UserDataManager _userDataManager;
        
        [Inject]
        private UIPrefabFactory _uiPrefabFactory;

        private FishesConfig _fishesConfig;
        
        private void Start()
        {
            _fishesConfig = _configsProvider.Get<FishesConfig>();
            foreach (var item in _list)
            {
                item.OnClicked += data =>
                {
                    var popup = _uiPrefabFactory.CreatePopup<FishDataPopup>();
                    popup.SetData(data);
                };
            }
        }

        public override void ProcessScreenOpen()
        {
            var libraryData = _userDataManager.GetData().UserLibraryData;

            for (int i = 0; i < _list.Count; i++)
            {
                var fishData = _fishesConfig.Fishes[i];
                bool locked = !libraryData.UnlockedFishes.Contains(i);
                _list[i].Initialize(fishData, locked);
            }
        }
    }
}