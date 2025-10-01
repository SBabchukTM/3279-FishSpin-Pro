using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI.Extensions;

namespace UI
{
    public class LineGraphRenderer : MonoBehaviour
    {
        [SerializeField] private RectTransform _graphArea;
    
        //5 entries
        [SerializeField] private TextMeshProUGUI[] _weightTexts;

        [SerializeField] private GameObject _circlePrefab;
        [SerializeField] private GameObject _noDataError;
    
        private UILineRenderer _lineRenderer;
    
        private List<GameObject> _circles = new();

        private void Start()
        {
            _lineRenderer = GameObject.FindObjectOfType<UILineRenderer>(true);
            _lineRenderer.gameObject.SetActive(false);
        }

        public void SetError()
        {
            _noDataError.gameObject.SetActive(true);
            _lineRenderer.gameObject.SetActive(false);
        
            for(int i = 0; i < _weightTexts.Length; i++)
                _weightTexts[i].gameObject.SetActive(false);
        
            for (int i = 0; i < _circles.Count; i++) 
                Destroy(_circles[i]);
            _circles.Clear();
        }

        public void PaintGraph(float[] weights)
        {
            _noDataError.SetActive(false);
        
            for (int i = 0; i < _circles.Count; i++) 
                Destroy(_circles[i]);
        
            for(int i = 0; i < _weightTexts.Length; i++)
                _weightTexts[i].gameObject.SetActive(true);
        
            _circles.Clear();

            _lineRenderer.gameObject.SetActive(true);
            _lineRenderer.transform.localScale = Vector3.one / _lineRenderer.transform.parent.localScale.x;

            var corners = new Vector3[4];
            _graphArea.GetWorldCorners(corners);

            var bottomLeft = corners[0];
            var topRight = corners[2];

            var width = topRight.x - bottomLeft.x;
            var height = topRight.y - bottomLeft.y;

            var maxWeight = weights.Max();
            var minWeight = weights.Min();

            var weightRange = Mathf.Max(maxWeight - minWeight, 0.0001f);

            var daysMax = 6;
            var xStep = width / daysMax;

            var xPos = bottomLeft.x;

            _lineRenderer.Points = new Vector2[weights.Length];

            if (weights.Length == 1)
            {
                _weightTexts[0].text = weights[0].ToString("0.#");
                for (int i = 1; i < _weightTexts.Length; i++)
                    _weightTexts[i].text = Mathf.Lerp(minWeight, minWeight * 2, i / 4f).ToString("0.#");
            }
            else
            {
                for (int i = 0; i < _weightTexts.Length; i++)
                {
                    float t = (float)i / (_weightTexts.Length - 1);
                    float weightValue = Mathf.Lerp(minWeight, maxWeight, t);

                    _weightTexts[i].gameObject.SetActive(true);
                    _weightTexts[i].text = weightValue.ToString("0.#");
                }
            }
        
            for (var i = 0; i < weights.Length; i++)
            {
                float weight = weights[i];

                // Normalize weight to 0–1 range
                float t = (weight - minWeight) / weightRange;
                float yPos = Mathf.Lerp(bottomLeft.y, topRight.y, t);

                var circle = Instantiate(_circlePrefab, transform);
                circle.transform.position = new Vector3(xPos, yPos);

                _lineRenderer.Points[i] = new Vector2(xPos, yPos);
                xPos += xStep;

                _circles.Add(circle.gameObject);
            }
        }
    }
}