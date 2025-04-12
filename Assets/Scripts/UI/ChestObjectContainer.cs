using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class ChestObjectContainer : MonoBehaviour
{

    [Header("Elements")]
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI nameText;


    [Header("Stats")]
    [SerializeField] private Transform statContainersParent;
    [field: SerializeField] public Button TakeButton { get; private set; }
    [field: SerializeField] public Button RecycleButton { get; private set; }
    [SerializeField] public TextMeshProUGUI recyclePriceText;


    [Header("Colour")]
    [SerializeField] private Image[] levelDependedImages;
    [SerializeField] private Image outline;


    public void Configure(ObjectDataSO objectData)
    {
        icon.sprite = objectData.Icon;
        nameText.text = objectData.name;
        recyclePriceText.text = objectData.RecyclePrice.ToString();

        Color imageColor = ColourHolder.GetColour(objectData.Rarity);
        nameText.color = imageColor;

        outline.color = ColourHolder.GetOutlineColour(objectData.Rarity);

        foreach (Image image in levelDependedImages)
            image.color = imageColor;

        ConfigureStatContainers(objectData.baseStats);

    }

    private void ConfigureStatContainers(Dictionary<Stat, float> Stats)
    {
        
        StatContainerManager.GenerateStatContainers(Stats, statContainersParent);
    }

}
