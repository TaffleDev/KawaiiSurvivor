using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISelector : MonoBehaviour
{

    [Header("Elements")]
    [SerializeField] private EventSystem eventSystem;

    [Header("Panels")]
    [SerializeField] private Panel shopPanel;
    private void Awake()
    {
        UIManager.OnPanelShown += PanelShownCallback;
        
        InputManager.onLockAction += LockCallback;

        ShopManager.onItemPurchased += ItemPurchasedCallback;
        ShopManager.onRerollDisabled += RerollDisabledCallback;

        ShopManagerUI.OnInventoryOpenedAction += InventoryOpenedCallback;
        ShopManagerUI.OnInventoryClosedAction += InventoryClosedCallback;
        
        ShopManagerUI.OnPlayerStatsOpenedAction += PlayerStatsOpenedCallback;
        ShopManagerUI.OnPlayerStatsClosedAction += PlayerStatsClosedCallback;
        ShopManagerUI.onItemInfoClosedAction += ItemInfoClosedCallback;

        InventoryManager.onItemInfoOpenedAction += ItemInfoOpenedCallback;
        InventoryManager.OnItemRecycledAction += ItemRecycledCallback;
        InventoryManager.onWeaponMergedAction += WeaponMergedCallBack;

        ChestObjectContainer.onSpawned += ChestSpawnedCallback;

        WaveTransitionManager.onConfigured += WaveTransitionConfiguredCallback;

    }

    

    private void OnDestroy()
    {
        UIManager.OnPanelShown -= PanelShownCallback;
        
        InputManager.onLockAction -= LockCallback;
        
        ShopManager.onItemPurchased -= ItemPurchasedCallback;
        ShopManager.onRerollDisabled -= RerollDisabledCallback;
        
        ShopManagerUI.OnInventoryOpenedAction -= InventoryOpenedCallback;
        ShopManagerUI.OnInventoryClosedAction -= InventoryClosedCallback;
        ShopManagerUI.OnPlayerStatsOpenedAction -= PlayerStatsOpenedCallback;
        ShopManagerUI.OnPlayerStatsClosedAction -= PlayerStatsClosedCallback;
        ShopManagerUI.onItemInfoClosedAction -= ItemInfoClosedCallback;

        InventoryManager.onItemInfoOpenedAction -= ItemInfoOpenedCallback;
        InventoryManager.OnItemRecycledAction -= ItemRecycledCallback;
        InventoryManager.onWeaponMergedAction -= WeaponMergedCallBack;
        
        ChestObjectContainer.onSpawned -= ChestSpawnedCallback;

        WaveTransitionManager.onConfigured -= WaveTransitionConfiguredCallback;
        
    }
    
    private void ChestSpawnedCallback(GameObject takeButton)
    {
        SetSelectedGameObject(takeButton);
    }

    private void WaveTransitionConfiguredCallback(GameObject upgradeContainer)
    {
        SetSelectedGameObject(upgradeContainer);
    }
    
    
    
    private void WeaponMergedCallBack(GameObject recycleButton)
    {
        SetSelectedGameObject(recycleButton);
    }


    private void ItemInfoClosedCallback()
    {
        GameObject selected = InventoryManager.instance.GetFirstItem();
        
        if (selected != null)
            SetSelectedGameObject(selected);
    }

    private void ItemInfoOpenedCallback(Button recycleButton)
    {
        SetSelectedGameObject(recycleButton.gameObject);
    }

    private void ItemRecycledCallback(GameObject inventoryFirstItem)
    {
        if (inventoryFirstItem != null)
            SetSelectedGameObject(inventoryFirstItem);
    }

    
    
    private void LockCallback()
    {
        if (eventSystem.currentSelectedGameObject == null)
            return;
        
        GameObject go = eventSystem.currentSelectedGameObject;
        
        if (go.TryGetComponent(out ShopItemContainer itemContainer))
            itemContainer.LockButtonCallback();
    }
    
    private void ItemPurchasedCallback()
    {
        SelectShopPanelFirstObject();
    }
    
    private void RerollDisabledCallback()
    {
        SelectShopPanelFirstObject();
    }
    
    private void InventoryOpenedCallback()
    {
        UIManager.SetPanelInteractAbility(shopPanel.gameObject, false);

        GameObject selected = InventoryManager.instance.GetFirstItem();
        
        if (selected != null)
            SetSelectedGameObject(selected);
    }
    
    private void InventoryClosedCallback()
    {
        UIManager.SetPanelInteractAbility(shopPanel.gameObject, true);
        SelectShopPanelFirstObject();

    }
    
    private void PlayerStatsOpenedCallback()
    {
        UIManager.SetPanelInteractAbility(shopPanel.gameObject, false);
        
        
    }

    private void PlayerStatsClosedCallback()
    {
        UIManager.SetPanelInteractAbility(shopPanel.gameObject, true);
        SelectShopPanelFirstObject();
    }
    
    private void SelectShopPanelFirstObject()
    {
        if (shopPanel.FirstSelectedObject != null)
            SetSelectedGameObject(shopPanel.FirstSelectedObject);
    }
    
    private void PanelShownCallback(Panel panel)
    {
        if (panel.FirstSelectedObject != null)
            SetSelectedGameObject(panel.FirstSelectedObject);
    }

    
    

    private void SetSelectedGameObject(GameObject go) => eventSystem.SetSelectedGameObject(go);
}
