using System;
using UnityEngine;


[RequireComponent (typeof(EnemyMovement), typeof(RangeEnemyAttack))]
public class RangeEnemy : MonoBehaviour
{
    [Header("Components")]
    private EnemyMovement enemyMovement;
    private RangeEnemyAttack attack;

    [Header("Health")]
    [SerializeField] private int maxHealth;
    int health;


    [Header("Elements")]
    private Player player;

    [Header("Spawn Dequence Related")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private SpriteRenderer spawnIndicator;
    [SerializeField] private Collider2D spriteCollider;
    private bool hasSpawned;

    [Header("Effects")]
    [SerializeField] private ParticleSystem passAwayParticle;


    [Header("Attack")]
    [SerializeField] private float playerDetectionRadius;

    [Header("Actions")]
    public static Action<int, Vector2> onDamageTaken;

    [Header("DEBUG")]
    [SerializeField] private bool showGizmos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = maxHealth;

        enemyMovement = GetComponent<EnemyMovement>();
        attack = GetComponent<RangeEnemyAttack>();
        player = FindFirstObjectByType<Player>();

        attack.StorePlayer(player);

        if (player == null)
        {
            Debug.LogWarning("No player found, auto-destroying...");
            Destroy(gameObject);
        }

        StartSpawnSequence();


    }

    // Update is called once per frame
    void Update()
    {
        if (!spriteRenderer.enabled)
            return;

        ManageAttack();               
    }

    private void ManageAttack()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        if (distanceToPlayer > playerDetectionRadius)
            enemyMovement.FollowPlayer();
        else
            TryAttack();
    }

    private void StartSpawnSequence()
    {
        //spriteRenderer.enabled = false;
        //spawnIndicator.enabled = true;
        SetRenderersVisibility(false);
        Vector3 targetScale = spawnIndicator.transform.localScale * 1.2f;
        LeanTween.scale(spawnIndicator.gameObject, targetScale, .3f)
            .setLoopPingPong(4)
            .setOnComplete(SpawnSequenceCompleted);

    }

    private void SpawnSequenceCompleted()
    {
        //spriteRenderer.enabled = true;
        //spawnIndicator.enabled = false;
        SetRenderersVisibility(true);
        hasSpawned = true;

        spriteCollider.enabled = true;

        enemyMovement.StorePlayer(player);
    }

    private void SetRenderersVisibility(bool visibility)
    {
        spriteRenderer.enabled = visibility;
        spawnIndicator.enabled = !visibility;
    }      

    private void TryAttack()
    {
        attack.AutoAim();
    }   

    public void TakeDamage(int damage)
    {
        int realDamage = Mathf.Min(damage, health);
        health -= realDamage;

        onDamageTaken?.Invoke(damage, transform.position);

        if (health <= 0)
            PassAway();
    }

    private void PassAway()
    {
        passAwayParticle.transform.SetParent(null);
        passAwayParticle.Play();

        Destroy(gameObject);

    }

    private void OnDrawGizmos()
    {
        if (!showGizmos)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, playerDetectionRadius);

    }
}
