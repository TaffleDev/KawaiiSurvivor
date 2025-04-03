using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatContainer : MonoBehaviour
{

    [Header("Elements")]
    [SerializeField] private Image statImage;
    [SerializeField] private TextMeshProUGUI statText;
    [SerializeField] private TextMeshProUGUI statValueText;


    
    public void Configure(Sprite icon, string statname, string statvalue)
    {
        statImage.sprite = icon;
        statText.text = statname;
        statValueText.text = statvalue;
    }
}
