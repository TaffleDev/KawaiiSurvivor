using UnityEngine;

public class WeaponSelectionManager : MonoBehaviour, IGameStateListener
{

    [Header("Elements")]
    [SerializeField] private Transform containersParent;
    [SerializeField] private WeaponSelectionContainer weaponContainerPrefab;


    [Header("Data")]
    [SerializeField] private WeaponDataSO[] starterWeapons;


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
        switch (gameState)
        {
            case GameState.WEAPONSELECTION:
                Configure();
                break;
        }
    }

    [NaughtyAttributes.Button]
    private void Configure()
    {
        containersParent.Clear();

        for (int i = 0; i < 3; i++)
            GenerateWeaponContainer();
    }


    private void GenerateWeaponContainer()
    {
        WeaponSelectionContainer containerInstance = Instantiate(weaponContainerPrefab, containersParent);

        WeaponDataSO weaponData = starterWeapons[Random.Range(0, starterWeapons.Length)];

        int level = Random.Range(0, 5);


        containerInstance.Configure(weaponData.Sprite, weaponData.Name, level);

        containerInstance.Button.onClick.RemoveAllListeners();
        containerInstance.Button.onClick.AddListener(() => WeaponSelectedCallback(containerInstance, weaponData));
    }

    private void WeaponSelectedCallback(WeaponSelectionContainer containerInstance, WeaponDataSO weaponData)
    {
        foreach (WeaponSelectionContainer container in containersParent.GetComponentsInChildren<WeaponSelectionContainer>())
        {
            if (container == containerInstance)
                container.Slect();
            else
                container.Deselect();
        }
    }

}
