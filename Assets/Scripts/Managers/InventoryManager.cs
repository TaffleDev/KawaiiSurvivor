using System;
using UnityEngine;

public class InventoryManager : MonoBehaviour, IGameStateListener
{
    
    [Header("Player Components")]
    [SerializeField] private PlayerWeapons playerWeapons;
    [SerializeField] private PlayerObjects playerObjects;

    [Header("Elements")]
    [SerializeField] private Transform inventoryItemsParent;
    [SerializeField] private Transform pauseInventoryItemsParent;
    [SerializeField] private InventoryItemContainer inventoryItemContainer;
    [SerializeField] private ShopManagerUI shopManagerUI;
    [SerializeField] private InventoryItemInfo itemInfo;

    private void Awake()
    {
        ShopManager.onItemPurchased += ItemPurchasedCallBack;
        WeaponMerger.onMerge += WeaponMergedCallback;

        GameManager.onGamePaused += Configure;
    }

    private void OnDestroy()
    {
        ShopManager.onItemPurchased -= ItemPurchasedCallBack;
        WeaponMerger.onMerge -= WeaponMergedCallback;
        
        GameManager.onGamePaused -= Configure;
    }

    


    public void GameStateChangedCallBack(GameState gameState)
    {
        if (gameState == GameState.SHOP)
            Configure();
    }

    private void Configure()
    {
        inventoryItemsParent.Clear();
        pauseInventoryItemsParent.Clear();
        //Player Weapons / Player Objects
        Weapon[] weapons = playerWeapons.GetWeapons();

        for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i] == null)
                continue;
            
            InventoryItemContainer container = Instantiate(inventoryItemContainer, inventoryItemsParent);
            container.Configure(weapons[i], i, () => ShowItemInfo(container));
            
            InventoryItemContainer pauseContainer = Instantiate(inventoryItemContainer, pauseInventoryItemsParent);
            pauseContainer.Configure(weapons[i], i, null);
        }
        

        ObjectDataSO[] objectDatas = playerObjects.Objects.ToArray();

        for (int i = 0; i < objectDatas.Length; i++)
        {
            InventoryItemContainer container = Instantiate(inventoryItemContainer, inventoryItemsParent);
            container.Configure(objectDatas[i], () => ShowItemInfo(container));
            
            InventoryItemContainer pauseContainer = Instantiate(inventoryItemContainer, pauseInventoryItemsParent);
            pauseContainer.Configure(objectDatas[i], null);
        }

    }

    private void ShowItemInfo(InventoryItemContainer container)
    {
        if (container.Weapon != null)
            ShowWeaponInfo(container.Weapon, container.Index);
        else 
            ShowObjectInfo(container.ObjectData);
    }
    
    

    private void ShowWeaponInfo(Weapon weapon, int index)
    {
        itemInfo.Configure(weapon);
        
        itemInfo.RecycleButton.onClick.RemoveAllListeners();
        itemInfo.RecycleButton.onClick.AddListener(() => RecycleWeapon(index));
        
        shopManagerUI.ShowItemInfo();
    }
    
    private void RecycleWeapon(int index)
    {
        playerWeapons.RecycleWeapons(index);
        
        Configure();
        
        shopManagerUI.HideItemInfo();
        
        
    }
    
    private void ShowObjectInfo(ObjectDataSO objectData)
    {
        itemInfo.Configure(objectData);
        
        itemInfo.RecycleButton.onClick.RemoveAllListeners();
        itemInfo.RecycleButton.onClick.AddListener(() => RecycleObject(objectData));
        
        shopManagerUI.ShowItemInfo();
    }

    private void RecycleObject(ObjectDataSO objectToRecycle)
    {
        playerObjects.RecycleObjects(objectToRecycle);
        
        Configure();
        
        shopManagerUI.HideItemInfo();
    }

    private void ItemPurchasedCallBack() => Configure();
    
    private void WeaponMergedCallback(Weapon mergedWeapon)
    {
        Configure();
        itemInfo.Configure(mergedWeapon);
    }

}
