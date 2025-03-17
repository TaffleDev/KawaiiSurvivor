using System;
using UnityEngine;

public class DropManger : MonoBehaviour
{

    [Header("Elements")]
    [SerializeField] private Candy candyPrefab;

    private void Awake()
    {
        Enemy.onPassedAway += EnemyPassedAwayCallback;
    }

    private void OnDestroy()
    {
        Enemy.onPassedAway -= EnemyPassedAwayCallback;

    }
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void EnemyPassedAwayCallback(Vector2 enemyPostion)
    {
        Instantiate(candyPrefab, enemyPostion, Quaternion.identity, transform);
    }
}
