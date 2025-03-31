using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class WeaponSelectionContainer : MonoBehaviour
{

    [Header("Elements")]
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI nameText;


    [Header("Colour")]
    [SerializeField] private Image[] levelDependedImages;

    [field: SerializeField] public Button Button { get; private set; }

    public void Configure(Sprite sprite, string name, int level)
    {
        icon.sprite = sprite;
        nameText.text = name;

        Color imageColor = ColourHolder.GetColour(level);        

        foreach (Image image in levelDependedImages)
        {
            image.color = imageColor;
        }

    }

    public void Slect()
    {
        LeanTween.cancel(gameObject);
        LeanTween.scale(gameObject, Vector3.one * 1.075f, .3f).setEase(LeanTweenType.easeInSine);
    }

    public void Deselect()
    {
        LeanTween.cancel(gameObject);
        LeanTween.scale(gameObject, Vector3.one, .3f);
    }

    
}
