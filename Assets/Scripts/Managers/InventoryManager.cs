using UnityEngine;

public class InventoryManager : MonoBehaviour, IGameStateListener
{
    
    [Header("Player Components")]
    [SerializeField] private PlayerWeapons playerWeapons;
    [SerializeField] private PlayerObjects playerObjects;

    [Header("Elements")]
    [SerializeField] private Transform inventoryItemsParent;
    [SerializeField] private InventoryItemContainer inventoryItemContainer;
    [SerializeField] private ShopManagerUI shopManagerUI;
    [SerializeField] private InventoryItemInfo itemInfo;
    


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void GameStateChangedCallBack(GameState gameState)
    {
        if (gameState == GameState.SHOP)
            Configure();
    }

    private void Configure()
    {
        inventoryItemsParent.Clear();

        //Player Weapons / Player Objects
        Weapon[] weapons = playerWeapons.GetWeapons();

        for (int i = 0; i < weapons.Length; i++)
        {
            InventoryItemContainer container = Instantiate(inventoryItemContainer, inventoryItemsParent);
            container.Configure(weapons[i], () => ShowItemInfo(container));
        }

        

        ObjectDataSO[] objectDatas = playerObjects.Objects.ToArray();

        for (int i = 0; i < objectDatas.Length; i++)
        {
            InventoryItemContainer container = Instantiate(inventoryItemContainer, inventoryItemsParent);
            container.Configure(objectDatas[i], () => ShowItemInfo(container));
        }

    }

    private void ShowItemInfo(InventoryItemContainer container)
    {
        if (container.Weapon != null)
            ShowWeaponInfo(container.Weapon);
        else 
            ShowObjectInfo(container.ObjectData);

        
    }
    
    

    // ReSharper disable Unity.PerformanceAnalysis
    private void ShowWeaponInfo(Weapon weapon)
    {
        itemInfo.Configure(weapon);
        shopManagerUI.ShowItemInfo();
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void ShowObjectInfo(ObjectDataSO objectData)
    {
        itemInfo.Configure(objectData);
        shopManagerUI.ShowItemInfo();
    }
}
