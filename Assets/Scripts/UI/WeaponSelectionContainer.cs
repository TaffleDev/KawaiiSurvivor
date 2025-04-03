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
    [SerializeField] private StatContainer statContainerPrefab;
    [SerializeField] private Sprite statIcon;


    [Header("Colour")]
    [SerializeField] private Image[] levelDependedImages;

    [field: SerializeField] public Button Button { get; private set; }

    public void Configure(Sprite sprite, string name, int level, WeaponDataSO weaponData)
    {
        icon.sprite = sprite;
        nameText.text = name;

        Color imageColor = ColourHolder.GetColour(level);        

        foreach (Image image in levelDependedImages)
            image.color = imageColor;


        ConfigureStatContainers(weaponData);

    }

    private void ConfigureStatContainers(WeaponDataSO weaponData)
    {
        foreach (KeyValuePair<Stat, float> kvp in weaponData.baseStats)
        {
            StatContainer containerInstance = Instantiate(statContainerPrefab, statContainersParent);

            Sprite icon = ResourcesManager.GetStatIcon(kvp.Key);
            string statName = Enums.FormatStatName(kvp.Key);
            string statValue = kvp.Value.ToString();

            containerInstance.Configure(icon, statName, statValue);
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
