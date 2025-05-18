using System;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour, IGameStateListener
{
    public static InventoryManager instance;
    
    [Header("Player Components")]
    [SerializeField] private PlayerWeapons playerWeapons;
    [SerializeField] private PlayerObjects playerObjects;

    [Header("Elements")]
    [SerializeField] private Transform inventoryItemsParent;
    [SerializeField] private Transform pauseInventoryItemsParent;
    [SerializeField] private InventoryItemContainer inventoryItemContainer;
    [SerializeField] private ShopManagerUI shopManagerUI;
    [SerializeField] private InventoryItemInfo itemInfo;

    [Header("Actions")]
    public static Action<Button> onItemInfoOpenedAction;
    public static Action<GameObject> OnItemRecycledAction;
    public static Action<GameObject> onWeaponMergedAction;
    
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        
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
        
        onItemInfoOpenedAction?.Invoke(itemInfo.RecycleButton);
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
        
        OnItemRecycledAction?.Invoke(GetFirstItem());
    }

    private void ItemPurchasedCallBack() => Configure();
    
    private void WeaponMergedCallback(Weapon mergedWeapon)
    {
        Configure();
        itemInfo.Configure(mergedWeapon);
        
        onWeaponMergedAction?.Invoke(itemInfo.RecycleButton.gameObject);
    }

    public GameObject GetFirstItem()
    {
        if (inventoryItemsParent.childCount > 0)
            return inventoryItemsParent.GetChild(0).gameObject;
        
        return null;
    }

}
