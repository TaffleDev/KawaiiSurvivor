using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatContainer : MonoBehaviour
{

    [Header("Elements")]
    [SerializeField] private Image statImage;
    [SerializeField] private TextMeshProUGUI statText;
    [SerializeField] private TextMeshProUGUI statValueText;


    
    public void Configure(Sprite icon, string statname, float statvalue, bool useColor = false)
    {
        statImage.sprite = icon;
        statText.text = statname;

        if (useColor)
            ColourRizeStatValueText(statvalue);
        else
        {
            statValueText.color = Color.white;
            statValueText.text = statvalue.ToString("F2");
        }
    }

    private void ColourRizeStatValueText(float statValue)
    {

        float sign = Mathf.Sign(statValue);

        if (statValue == 0)
            sign = 0;

        float abtStatValue = Mathf.Abs(statValue);

        Color statValueTextColor = Color.white;

        if (sign > 0)
            statValueTextColor = Color.green;
        else if (sign < 0)
            statValueTextColor = Color.red;


        statValueText.color = statValueTextColor;
        statValueText.text = abtStatValue.ToString("F2");
    }

    public float GetFontSize()
    {
        return statText.fontSize;
    }

    public void SetFontSize(float fontSize)
    {
        statText.fontSizeMax = fontSize;
        statValueText.fontSizeMax = fontSize;
    }
}
