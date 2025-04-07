using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

using Random = UnityEngine.Random;

public class PlayerHealth : MonoBehaviour, IPlayerStatsDependency
{
    [Header("Settings")]
    [SerializeField] private int baseMaxHealth;
    private float maxHealth;
    float health;
    private float armor;
    private float lifeSteal;
    private float dodge;
    private float healthRecoverySpeed;
    private float healthRecoveryValue;
    private float healthRecoveryTimer;
    private float healthRecoveryDuration;

    [Header("Elements")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TextMeshProUGUI healthText;


    [Header("Actions")]
    public static Action<Vector2> onAttackDodged;


    private void Awake()
    {
        Enemy.onDamageTaken += EnemyTookDamageCallback;
    }

    private void OnDestroy()
    {
        Enemy.onDamageTaken -= EnemyTookDamageCallback;
    }

    private void EnemyTookDamageCallback(int damage, Vector2 enemyPos, bool isCriticalHit)
    {
        if (health >= maxHealth)
            return;

        float lifeStealValue = damage * lifeSteal;
        float healthToAdd = Mathf.Min(lifeStealValue, maxHealth - health);

        health += healthToAdd;
        UpdateUI();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (health < maxHealth)
            Recoverhealth();
    }

    private void Recoverhealth()
    {
        healthRecoveryTimer += Time.deltaTime;
        
        if(healthRecoveryTimer >= healthRecoveryDuration)
        {
            healthRecoveryTimer = 0;

            float healthToAdd = Mathf.Min(.1f, maxHealth - health);
            health += healthToAdd;

            UpdateUI();
        }
    }

    public void TakeDamage(int damage)
    {
        if (ShouldDodge())
        {
            onAttackDodged?.Invoke(transform.position);
            return;
        }

        float realDamage = damage * Mathf.Clamp(1 - (armor / 1000), 0, 10000);
        realDamage = Mathf.Min(realDamage, health);
        health -= realDamage;

        UpdateUI();


        if (health <= 0)
        {
            PassAway();
        }
    }

    private bool ShouldDodge()
    {
        return Random.Range(0f, 100f) < dodge;
    }

    private void PassAway()
    {
        GameManager.instance.SetGameState(GameState.GAMEOVER);
    }
    private void UpdateUI()
    {
        float healthBarValue = health / maxHealth;
        healthSlider.value = healthBarValue;
        healthText.text = (int)health + " / " + maxHealth;
    }

    public void UpdateStats(PlayerStatsManager playerStatsManager)
    {
        float addedHealth = playerStatsManager.GetStatValue(Stat.MaxHealth);
        maxHealth = baseMaxHealth + (int)addedHealth;
        maxHealth = Mathf.Max(maxHealth, 1);

        health = maxHealth;
        UpdateUI();

        armor = playerStatsManager.GetStatValue(Stat.Armor);
        lifeSteal = playerStatsManager.GetStatValue (Stat.LifeSteal) / 100;
        dodge = playerStatsManager.GetStatValue(Stat.Dodge);

        healthRecoverySpeed = Mathf.Max(.0001f, playerStatsManager.GetStatValue(Stat.HealthRecoverySpeed));
        healthRecoveryDuration = 1f / healthRecoverySpeed;
    }
}
