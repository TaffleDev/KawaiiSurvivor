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
    [SerializeField] private Image outline;


    public void Configure(WeaponDataSO weaponData, int level )
    {
        icon.sprite = weaponData.Sprite;
        nameText.text = weaponData.name + $" (lvl {level + 1})";

        Color imageColor = ColourHolder.GetColour(level);
        nameText.color = imageColor;

        outline.color = ColourHolder.GetOutlineColour(level);

        foreach (Image image in levelDependedImages)
            image.color = imageColor;

        Dictionary<Stat, float> calculatedStats = WeaponStatsCalculator.GetStats(weaponData, level);
        ConfigureStatContainers(calculatedStats);

    }

    private void ConfigureStatContainers(Dictionary<Stat, float> calculatedStats)
    {
        StatContainerManager.GenerateStatContainers(calculatedStats, statContainersParent);
    }

    public void Select()
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
