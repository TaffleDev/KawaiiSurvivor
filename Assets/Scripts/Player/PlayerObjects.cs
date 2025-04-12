using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[RequireComponent(typeof(PlayerStatsManager))]
public class PlayerObjects : MonoBehaviour
{
    [field: SerializeField] public List<ObjectDataSO> Objects {  get; private set; }
    private PlayerStatsManager playerStatsManager;

    private void Awake()
    {
        playerStatsManager = GetComponent<PlayerStatsManager>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (ObjectDataSO objectData in Objects)
        {
            playerStatsManager.AddObject(objectData.baseStats);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddObjects(ObjectDataSO objectData)
    {
        Objects.Add(objectData);
        playerStatsManager.AddObject(objectData.baseStats);

    }

}
