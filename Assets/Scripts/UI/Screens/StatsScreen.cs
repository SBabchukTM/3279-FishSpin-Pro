using System;
using System.Collections.Generic;
using System.Linq;
using AssetsProvider;
using Configs;
using DG.Tweening;
using Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
using Zenject;
using Object = UnityEngine.Object;

namespace UI.Screens
{
    public class StatsScreen : SelectableScreen
    {
        [SerializeField] private CanvasGroup _pieScreen;
        [SerializeField] private CanvasGroup _lineScreen;
        
        [SerializeField] private RectTransform _pieChartRowsContent;
        [SerializeField] private GameObject _rowPrefab;
        [SerializeField] private RectTransform _pieChartContainer;
        [SerializeField] private GameObject _pieSlicePrefab;
        [SerializeField] private MonthSelectButton _pieScreenButton;
        [SerializeField] private MonthSelectButton _lineScreenButton;
        [SerializeField] private FishTypeChartSelector _fishTypeChartSelector;
        [SerializeField] private TextMeshProUGUI _noDataText;
        
        [Inject] private UserDataManager _userDataManager;

        [Inject] private ConfigsProvider _configsProvider;

        private readonly List<GameObject> _spawnedPieCharRows = new();
        private readonly List<GameObject> _spawnedPieSlices = new();

        private Sequence _sequence;
        
        private void Start()
        {
            _pieScreenButton.OnClick += PieScreenButtonOnOnClick;
            _lineScreenButton.OnClick += LineScreenButtonOnOnClick;
        }

        private void OnDestroy()
        {
            _pieScreenButton.OnClick -= PieScreenButtonOnOnClick;
            _lineScreenButton.OnClick -= LineScreenButtonOnOnClick;
        }

        private void LineScreenButtonOnOnClick()
        {
            _sequence?.Kill();
            _sequence = DOTween.Sequence();
            
            _pieScreen.blocksRaycasts = false;
            _lineScreen.blocksRaycasts = false;

            _pieScreenButton.SetActive(false);
            _lineScreenButton.SetActive(true);
            
            _sequence.Append(_pieScreen.DOFade(0, 0.5f));
            _sequence.Join(_lineScreen.DOFade(1, 0.5f));

            _sequence.SetLink(gameObject);
            _sequence.OnComplete(() =>
            {
                _lineScreen.blocksRaycasts = true;
            });
            
            _fishTypeChartSelector.TrySetDataOnOpen();
            Object.FindObjectOfType<UILineRenderer>(true).transform.SetAsLastSibling();
        }

        private void PieScreenButtonOnOnClick()
        {
            _sequence?.Kill();
            _sequence = DOTween.Sequence();
            
            _pieScreen.blocksRaycasts = false;
            _lineScreen.blocksRaycasts = false;
            
            _pieScreenButton.SetActive(true);
            _lineScreenButton.SetActive(false);
            
            _sequence.Append(_pieScreen.DOFade(1, 0.5f));
            _sequence.Join(_lineScreen.DOFade(0, 0.5f));

            _sequence.SetLink(gameObject);
            _sequence.OnComplete(() =>
            {
                _pieScreen.blocksRaycasts = true;
            });
            
            Object.FindObjectOfType<UILineRenderer>(true).gameObject.SetActive(false);
        }

        public override void ProcessScreenOpen()
        {
            base.ProcessScreenOpen();

            DestroyRows();
            DestroyPies();

            var dict = GetFishOccurancesDict();
            SpawnRows(dict);
            CreatePieChart(dict);
            
            if (_lineScreen.blocksRaycasts)
            {
                _fishTypeChartSelector.TrySetDataOnOpen();
                var line = Object.FindObjectOfType<UILineRenderer>(true);
                line.transform.SetAsLastSibling();
            }
        }

        public override void ProcessScreenClose()
        {
            base.ProcessScreenClose();
            Object.FindObjectOfType<UILineRenderer>(true).gameObject.SetActive(false);
        }

        private void DestroyRows()
        {
            foreach (var row in _spawnedPieCharRows)
                Destroy(row.gameObject);
            _spawnedPieCharRows.Clear();
        }

        private void DestroyPies()
        {
            foreach (var row in _spawnedPieSlices)
                Destroy(row.gameObject);
            _spawnedPieSlices.Clear();
        }

        private Dictionary<int, int> GetFishOccurancesDict()
        {
            var records = _userDataManager.GetData().UserRecordsData.Records;
            Dictionary<int, int> _fishOccurances = new();

            foreach (var record in records)
                if (!_fishOccurances.TryAdd(record.FishId, 1))
                    _fishOccurances[record.FishId]++;
            return _fishOccurances;
        }

        private void SpawnRows(Dictionary<int, int> fishOccurances)
        {
            var list = _configsProvider.Get<FishesConfig>().Fishes;

            _noDataText.gameObject.SetActive(fishOccurances.Count == 0);
            
            foreach (var fishId in fishOccurances.Keys)
            {
                var row = Instantiate(_rowPrefab).GetComponent<FishStatisticRow>();
                row.SetData(list[fishId]);
                row.transform.SetParent(_pieChartRowsContent, false);

                _spawnedPieCharRows.Add(row.gameObject);
            }
        }

        private void CreatePieChart(Dictionary<int, int> fishOccurances)
        {
            var list = _configsProvider.Get<FishesConfig>().Fishes;
            var totalCount = fishOccurances.Values.Sum();
            if (totalCount == 0)
                return;

            float currentAngle = 0f;

            foreach (var pair in fishOccurances)
            {
                var fishId = pair.Key;
                var count = pair.Value;
                var percentage = (float)count / totalCount;

                var sliceGO = Instantiate(_pieSlicePrefab, _pieChartContainer);
                var image = sliceGO.GetComponent<Image>();
                image.fillAmount = percentage;
                image.color = list[fishId].Color;
                
                sliceGO.transform.localRotation = Quaternion.Euler(0f, 0f, currentAngle);

                var label = sliceGO.GetComponentInChildren<TextMeshProUGUI>();
                if (label != null)
                {
                    label.text = $"{Mathf.RoundToInt(percentage * 100)}%";

                    float midAngleDeg = currentAngle + (percentage * 360f / 2f);
                    float midAngleRad = midAngleDeg * Mathf.Deg2Rad;
                    float radius = (_pieChartContainer.rect.width / 2f) * 0.65f;

                    Vector2 offset = new Vector2(Mathf.Cos(midAngleRad), Mathf.Sin(midAngleRad)) * radius;

                    label.transform.SetParent(_pieChartContainer, false);
                    label.rectTransform.anchoredPosition = offset;
                    label.rectTransform.localRotation = Quaternion.identity;
                    label.rectTransform.pivot = new Vector2(0.5f, 0.5f);
                    label.alignment = TextAlignmentOptions.Center;
                    
                    _spawnedPieCharRows.Add(label.gameObject);
                }
                
                currentAngle += percentage * 360f;
                _spawnedPieSlices.Add(sliceGO);
            }
        }
    }
}