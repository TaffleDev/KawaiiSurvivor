using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NaughtyAttributes;

using Random = UnityEngine.Random;

public class WaveTransitionManager : MonoBehaviour, IGameStateListener
{
    public static WaveTransitionManager instance;

    [Header("Player")]
    [SerializeField] private PlayerObjects playerObjects;

    [Header("Elements")]
    [SerializeField] private PlayerStatsManager playerStatsManager;
    [SerializeField] private GameObject upgradeContainersParent;
    [SerializeField] private UpgradeContainer[] upgradeContainers;


    [Header("Chest Related Stuff")]
    [SerializeField] private ChestObjectContainer chestContainerPrefab;
    [SerializeField] private Transform chestContainerParent;

    [Header("Settings")]
    private int chestsCollected;

    [Header("Actions")]
    public static Action<GameObject> onConfigured;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        Chest.onCollected += ChestCollectedCallBack;
    }

    private void OnDestroy()
    {
        Chest.onCollected -= ChestCollectedCallBack;
        
    }
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
            case GameState.WAVETRANSITION:
                TryOpenChest();
                break;
        }
    }

    private void TryOpenChest()
    {
        chestContainerParent.Clear();

        if (chestsCollected > 0)
            ShowObject();
        else
            ConfigureUpgradeContainers();
    }

    private void ShowObject()
    {
        chestsCollected--;

        upgradeContainersParent.SetActive(false);

        ObjectDataSO[] objectDatas = ResourcesManager.Objects;
        ObjectDataSO randomObjectData = objectDatas[Random.Range(0, objectDatas.Length)];

        ChestObjectContainer containerInstance = Instantiate(chestContainerPrefab, chestContainerParent);
        containerInstance.Configure(randomObjectData);

        containerInstance.TakeButton.onClick.AddListener(() => TakeButtonCallback(randomObjectData));
        containerInstance.RecycleButton.onClick.AddListener(() => RecycleButtonCallback(randomObjectData));
    }

    private void TakeButtonCallback(ObjectDataSO objectToTake)
    {
        playerObjects.AddObjects(objectToTake);

        TryOpenChest();
    }

    private void RecycleButtonCallback(ObjectDataSO objectToRecycle)
    {
        CurrencyManager.instance.AddCurrency(objectToRecycle.RecyclePrice);
        TryOpenChest();
    }


    [Button]
    private void ConfigureUpgradeContainers()
    {
        upgradeContainersParent.SetActive(true);


        for (int i = 0; i < upgradeContainers.Length; i++)
        {
            int randomIndex = Random.Range(0, Enum.GetValues(typeof(Stat)).Length);
            Stat stat = (Stat)Enum.GetValues(typeof(Stat)).GetValue(randomIndex);

            Sprite upgradeSprite = ResourcesManager.GetStatIcon(stat);

            string randomStatString = Enums.FormatStatName(stat);

            string buttonString;
            Action action = GetActionToPerform(stat, out buttonString);

            upgradeContainers[i].Configure(upgradeSprite, randomStatString, buttonString);


            upgradeContainers[i].Button.onClick.RemoveAllListeners();
            upgradeContainers[i].Button.onClick.AddListener(() => action?.Invoke());
            upgradeContainers[i].Button.onClick.AddListener(() => BonusSelectedCallback());

        }
        
        onConfigured?.Invoke(upgradeContainers[0].gameObject);
    }

    private void BonusSelectedCallback()
    {
        GameManager.instance.WaveCompletedCallback();
    }

    private Action GetActionToPerform(Stat stat, out string buttonString)
    {
        buttonString = "";
        float value;

        value = Random.Range(1, 10);
        buttonString = "+" + value.ToString() + "%";

        switch (stat)
        {
            case Stat.Attack:
                value = Random.Range(1, 10);
                buttonString = "+" + value.ToString() + "%";
                break;

            case Stat.AttackSpeed:
                value = Random.Range(1, 10);
                buttonString = "+" + value.ToString() + "%";
                break;

            case Stat.CriticalChance:
                value = Random.Range(1, 10);
                buttonString = "+" + value.ToString() + "%";
                break;

            case Stat.CriticalPercent:
                value = Random.Range(1f, 2f);
                buttonString = "+" + value.ToString("F2") + "x";
                break;

            case Stat.MoveSpeed:
                value = Random.Range(1, 10);
                buttonString = "+" + value.ToString() + "%";
                break;

            case Stat.MaxHealth:
                value = Random.Range(1, 5);
                buttonString = "+" + value;
                break;

            case Stat.Range:
                value = Random.Range(1f, 5f);
                buttonString = "+" + value.ToString("F2");
                break;

            case Stat.HealthRecoverySpeed:
                value = Random.Range(1, 10);
                buttonString = "+" + value.ToString() + "%";
                break;

            case Stat.Armor:
                value = Random.Range(1, 10);
                buttonString = "+" + value.ToString() + "%";
                break;

            case Stat.Luck:
                value = Random.Range(1, 10);
                buttonString = "+" + value.ToString() + "%";
                break;

            case Stat.Dodge:
                value = Random.Range(1, 10);
                buttonString = "+" + value.ToString() + "%";
                break;

            case Stat.LifeSteal:
                value = Random.Range(1, 10);
                buttonString = "+" + value.ToString() + "%";
                break;

            default:
                return () => Debug.Log("Invalid stat");
        }

        //buttonString = Enums.FormatStatName(stat) + "\n" + buttonString;

        return () => playerStatsManager.AddPlayerStat(stat, value);

    }

    private void ChestCollectedCallBack()
    {
        chestsCollected++;
        // Debug.Log($" We now have {chestsCollected} chests!");
    }

    public bool HasCollectedChests() => chestsCollected > 0;
}
