using UnityEngine;
using UnityEngine.UI;

public class InventoryItemContainer : MonoBehaviour
{

    [Header("Elements")]
    [SerializeField] private Image container;
    [SerializeField] private Image icon;

    public void Configure(Color containerColor, Sprite itemIcon)
    {
        container.color = containerColor;
        icon.sprite = itemIcon;
    }
}
