using System.Linq;
using AssetsProvider;
using Configs;
using DG.Tweening;
using Services;
using TMPro;
using UI.Popups;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class FishTypeChartSelector : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private TextMeshProUGUI _fishText;
        [SerializeField] private LineGraphRenderer _lineGraphRenderer;
        [SerializeField] private TextMeshProUGUI _errorText;
        
        [Inject]
        private UserDataManager _userDataManager;
    
        [Inject]
        private UIPrefabFactory _uiPrefabFactory;
    
        [Inject]
        private ConfigsProvider _configsProvider;

        private int _lastFishId = -1;
    
        private Sequence _sequence;
        
        private void Awake()
        {
            _button.onClick.AddListener(SelectFish);
        }

        public void TrySetDataOnOpen()
        {
            if (_lastFishId == -1)
            {
                return;
            }
            PopupOnOnSelect(_lastFishId);
        }

        private void DisplayError()
        {
            _sequence?.Kill();
            _sequence = DOTween.Sequence();

            _sequence.Append(_errorText.DOFade(1, 0.2f));
            _sequence.AppendInterval(0.75f);
            _sequence.Append(_errorText.DOFade(0, 0.2f));
        }
    
        private void SelectFish()
        {
            var data = _userDataManager.GetData().UserRecordsData;
            if (data.Records.Count == 0)
            {
                _lineGraphRenderer.SetError();
                DisplayError();
                return;
            }

            var popup = _uiPrefabFactory.CreatePopup<FishTypeSelectPopup>();
        
            popup.OnSelect += PopupOnOnSelect;
        }

        private void PopupOnOnSelect(int fishId)
        {
            _fishText.text = _configsProvider.Get<FishesConfig>().Fishes[fishId].Name;
            var last7Records = _userDataManager.GetData().UserRecordsData.Records
                .Where(record => record.FishId == fishId)
                .OrderBy(record => record.ToDateTime())
                .TakeLast(7)
                .ToList();

            if (last7Records.Count == 0)
            {
                _lineGraphRenderer.SetError();
                return;
            }
        
            float[] weights = new float[last7Records.Count];
            for (int i = 0; i < last7Records.Count; i++) 
                weights[i] = float.Parse(last7Records[i].Weight);
        
            _lineGraphRenderer.PaintGraph(weights);
            _lastFishId = fishId;
        }
    }
}
