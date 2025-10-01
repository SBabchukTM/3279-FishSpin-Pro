using System;
using System.Collections.Generic;
using System.Linq;
using Services;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Popups
{
    public class FishTypeSelectPopup : BasePopup
    {
        [SerializeField] private List<Button> _fishButtons;

        public event Action<int> OnSelect;
        
        private void Awake()
        {
            for (int i = 0; i < _fishButtons.Count; i++)
            {
                int index = i;
                _fishButtons[i].onClick.AddListener(() =>
                {
                    OnSelect?.Invoke(index);
                    DestroyPopup();
                });
            }
        }

        public void DisableLockedFishes(List<UserFishRecord> records)
        {
            var uniqueFishIds = records
                .Select(x => x.FishId)
                .Distinct()
                .ToList();

            for (int i = 0; i < _fishButtons.Count; i++)
            {
                if(!uniqueFishIds.Contains(i))
                    _fishButtons[i].gameObject.transform.parent.gameObject.SetActive(false);
            }
        }
    }
}