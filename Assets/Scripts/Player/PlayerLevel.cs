using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class PlayerLevel : MonoBehaviour
{

    [Header("Settings")]
    private int requiredExp;
    private int currentExp;
    private int level;
    private int levelsEarnedThisWave;

    [Header("Visuals")]
    [SerializeField] private Slider expBar;
    [SerializeField] private TextMeshProUGUI levelText;

    private void Awake()
    {
        Candy.onCollected += CandyCollectedCallBack;
    }
    private void OnDestroy()
    {
        Candy.onCollected -= CandyCollectedCallBack;

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateRequiredExp();
        UpdateVisuals();

    }
    // Update is called once per frame
    void Update()
    {

    }    

    private void UpdateRequiredExp()
    {
        requiredExp = (level + 1) * 5;
    }

    private void UpdateVisuals()
    {
        expBar.value = (float)currentExp / requiredExp;
        levelText.text = "Level " + (level + 1);
    }

    private void CandyCollectedCallBack(Candy candy)
    {
        currentExp++;

        if (currentExp >= requiredExp)
            LevelUp();

        UpdateVisuals();
    }

    private void LevelUp()
    {
        level++;
        levelsEarnedThisWave++;
        currentExp = 0;
        UpdateRequiredExp();

    }

    public bool HasLeveledUp()
    {
        if (levelsEarnedThisWave > 0)
        {
            levelsEarnedThisWave--;
            return true;
        }

        return false;
    }
}
