using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryItemInfo : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI recyclePriceText;

    [Header("Colors")] 
    [SerializeField] private Image container;

    [Header("Stats")]
    [SerializeField] private Transform statsParent;

    public void Configure(Weapon weapon)
    {
        Configure
            (   weapon.WeaponData.Sprite,
                weapon.WeaponData.Name + "(lvl " + (weapon.level + 1)+ ")", 
                ColourHolder.GetColour(weapon.level),
                WeaponStatsCalculator.GetRecyclePrice(weapon.WeaponData, weapon.level),
                WeaponStatsCalculator.GetStats(weapon.WeaponData, weapon.level)
                );
    }

    public void Configure(ObjectDataSO objectData)
    {
        Configure
        (   objectData.Icon,
            objectData.Name, 
            ColourHolder.GetColour(objectData.Rarity),
            objectData.RecyclePrice,
            objectData.baseStats
        );
    }
    
    private void Configure(Sprite itemIcon, string name, Color containerColor, int recyclePrice, Dictionary<Stat, float> stats)
    {
        icon.sprite = itemIcon;
        itemNameText.text = name;
        itemNameText.color = containerColor;
        
        recyclePriceText.text = recyclePrice.ToString();
        
        container.color = containerColor;
        
        StatContainerManager.GenerateStatContainers(stats, statsParent);
        
    }
    
    
}
