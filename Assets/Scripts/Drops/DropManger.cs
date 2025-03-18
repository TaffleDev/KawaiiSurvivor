using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

//Sets the random to use te Unity Random instead of the C# random
using Random = UnityEngine.Random;

public class DropManger : MonoBehaviour
{

    [Header("Elements")]
    [SerializeField] private Candy candyPrefab;
    [SerializeField] private Cash cashPrefab;
    [SerializeField] private Chest chestPrefab;


    [Header("Setting")]
    [SerializeField] [Range(0, 100)] private int cashDropChance;
    [SerializeField] [Range(0, 100)] private int chestDropChance;


    [Header("Pooling")]
    private ObjectPool<Candy> candyPool;
    private ObjectPool<Cash> cashPool;

    private void Awake()
    {
        Enemy.onPassedAway += EnemyPassedAwayCallback;

        Candy.onCollected += ReleaseCandy;
        Cash.onCollected += ReleaseCash;
    }

    private void OnDestroy()
    {
        Enemy.onPassedAway -= EnemyPassedAwayCallback;

        Candy.onCollected -= ReleaseCandy;
        Cash.onCollected -= ReleaseCash;
    }
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        candyPool = new ObjectPool<Candy>(
            CandyCreateFunction,
            CandyActionOnGet,
            CandyActionOnRelease,
            CandyActionOnDestroy);

        cashPool = new ObjectPool<Cash>(
            CashCreateFunction,
            CashActionOnGet,
            CashActionOnRelease,
            CashActionOnDestroy);

    }


    #region Candy Actions
    // Expression Bodied Statement, They work when you have a method with only 1 line..
    // Look at the Cash Actions for comparison. They do the exact same
    private Candy CandyCreateFunction() => Instantiate(candyPrefab, transform);
    private void CandyActionOnGet(Candy candy) =>  candy.gameObject.SetActive(true);
    private void CandyActionOnRelease(Candy candy) => candy.gameObject.SetActive(false);
    private void CandyActionOnDestroy(Candy candy) => Destroy(candy.gameObject);
    #endregion

    #region Cash Actions
    private Cash CashCreateFunction()
    {
        Cash castInstance = Instantiate(cashPrefab, transform);
        return castInstance;
    }

    private void CashActionOnGet(Cash cash)
    {

        cash.gameObject.SetActive(true);
    }

    private void CashActionOnRelease(Cash cash)
    {
        cash.gameObject.SetActive(false);
    }

    private void CashActionOnDestroy(Cash cash)
    {
        Destroy(cash.gameObject);
    }
    #endregion

    // Update is called once per frame
    void Update()
    {
        
    }

    private void EnemyPassedAwayCallback(Vector2 enemyPostion)
    {
        bool shouldSpawnCash = Random.Range(0, 101) <= cashDropChance;

        DroppableCurrency droppable = shouldSpawnCash ? cashPool.Get() : candyPool.Get();
        droppable.transform.position = enemyPostion;

        TryDropChest(enemyPostion);

    }

    private void TryDropChest(Vector2 spawnPosition)
    {
        bool shouldSpawnChest = Random.Range(0, 101) <= chestDropChance;

        if (!shouldSpawnChest)
            return;

        Instantiate(chestPrefab, spawnPosition, Quaternion.identity);
    }

    private void ReleaseCandy(Candy candy) => candyPool.Release(candy);
    private void ReleaseCash(Cash cash) => cashPool.Release(cash);
}
