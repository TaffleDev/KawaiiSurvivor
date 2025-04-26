using UnityEngine;

public class InventoryManager : MonoBehaviour, IGameStateListener
{

    [Header("Player Components")]
    [SerializeField] private PlayerWeapons playerWeapons;
    [SerializeField] private PlayerObjects playerObjects;

    [Header("Elements")]
    [SerializeField] private Transform inventoryItemsParent;
    [SerializeField] private InventoryItemContainer inventoryItemContainer;

    


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

            Color containerColor = ColourHolder.GetColour(weapons[i].level);
            Sprite icon = weapons[i].WeaponData.Sprite;

            container.Configure(containerColor, icon);
        }

        

        ObjectDataSO[] objectDatas = playerObjects.Objects.ToArray();

        for (int i = 0; i < objectDatas.Length; i++)
        {
            InventoryItemContainer container = Instantiate(inventoryItemContainer, inventoryItemsParent);

            Color containerColor = ColourHolder.GetColour(objectDatas[i].Rarity);
            Sprite icon = objectDatas[i].Icon;

            container.Configure(containerColor, icon);
        }

    }
}
