using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;

public class WeaponSelectionContainer : MonoBehaviour
{

    [Header("Elements")]
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI nameText;


    [Header("Stats")]
    [SerializeField] private Transform statContainersParent;
    [field: SerializeField] public Button Button { get; private set; }


    [Header("Colour")]
    [SerializeField] private Image[] levelDependedImages;


    public void Configure(Sprite sprite, string name, int level, WeaponDataSO weaponData)
    {
        icon.sprite = sprite;
        nameText.text = name + $" (lvl {level + 1})";

        Color imageColor = ColourHolder.GetColour(level);
        nameText.color = imageColor;

        foreach (Image image in levelDependedImages)
            image.color = imageColor;

        Dictionary<Stat, float> calculatedStats = WeaponStatsCalculator.GetStats(weaponData, level);
        ConfigureStatContainers(calculatedStats);

    }

    private void ConfigureStatContainers(Dictionary<Stat, float> calculatedStats)
    {
        StatContainerManager.GenerateStatContainers(calculatedStats, statContainersParent);
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
