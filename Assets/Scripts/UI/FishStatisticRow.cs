using Configs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class FishStatisticRow : MonoBehaviour
    {
        [SerializeField] private Image _colorCircle;
        [SerializeField] private Image _fishImage;
        [SerializeField] private TextMeshProUGUI _nameText;

        public void SetData(FishData data)
        {
            _colorCircle.color = data.Color;
            _fishImage.sprite = data.Sprite;
            _nameText.text = data.Name;
        }
    }
}
