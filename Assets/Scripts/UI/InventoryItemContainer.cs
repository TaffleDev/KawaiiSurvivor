using System;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemContainer : MonoBehaviour
{

    [Header("Elements")]
    [SerializeField] private Image container;
    [SerializeField] private Image icon;
    [SerializeField] private Button button;

    public Weapon Weapon { get; private set; }
    public ObjectDataSO ObjectData { get; private set; }

    public void Configure(Color containerColor, Sprite itemIcon)
    {
        container.color = containerColor;
        icon.sprite = itemIcon;
    }


    public void Configure(Weapon weapon, Action clickedCallBack)
    {
        Weapon = weapon;

        Color color = ColourHolder.GetColour(weapon.level);
        Sprite icon = weapon.WeaponData.Sprite;

        Configure(color, icon);

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => clickedCallBack?.Invoke());
    }
    
    
    public void Configure (ObjectDataSO objectData, Action clickedCallBack)
    {
        ObjectData = objectData;

        Color color = ColourHolder.GetColour(objectData.Rarity);
        Sprite icon = objectData.Icon;

        Configure(color, icon);

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => clickedCallBack?.Invoke());
    }
}
