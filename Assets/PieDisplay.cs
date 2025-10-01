using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PieDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    
    public void SetPercent(float percent) => _text.text = percent.ToString("0.00") + "%";
}
