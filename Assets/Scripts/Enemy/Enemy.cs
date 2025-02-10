using UnityEngine;
using TMPro;

[RequireComponent (typeof(EnemyMovement))]
public class Enemy : MonoBehaviour
{
    [Header("Components")]
    private EnemyMovement enemyMovement;

    [Header("Health")]
    [SerializeField] private int maxHealth;
    int health;


    [Header("Elements")]
    private Player player;

    [Header("Spawn Dequence Related")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private SpriteRenderer spawnIndicator;
    private bool hasSpawned;

    [Header("Effects")]
    [SerializeField] private ParticleSystem passAwayParticle;


    [Header("Attack")]
    [SerializeField] private int damage;
    [SerializeField] private float attackFrequency;
    [SerializeField] private float playerDetectionRadius;

    private float attackDelay;
    private float attackTimer;

    [Header("DEBUG")]
    [SerializeField] private bool showGizmos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        health = maxHealth;

        enemyMovement = GetComponent<EnemyMovement>();
        player = FindFirstObjectByType<Player>();


        if (player == null)
        {
            Debug.LogWarning("No player found, auto-destroying...");
            Destroy(gameObject);
        }

        StartSpawnSequence();


        //Attack time
        attackDelay = 1f / attackFrequency;
    }

    // Update is called once per frame
    void Update()
    {
        if (attackTimer >= attackDelay)
            TryAttack();
        else
            Wait();
    }

    private void StartSpawnSequence()
    {
        spriteRenderer.enabled = false;
        spawnIndicator.enabled = true;

        Vector3 targetScale = spawnIndicator.transform.localScale * 1.2f;
        LeanTween.scale(spawnIndicator.gameObject, targetScale, .3f)
            .setLoopPingPong(4)
            .setOnComplete(SpawnSequenceCompleted);

    }

    private void SpawnSequenceCompleted()
    {        
        spriteRenderer.enabled = true;
        spawnIndicator.enabled = false;
        hasSpawned = true;

        enemyMovement.StorePlayer(player);
    }

    private void SetRenderersVisibility(bool visibility)
    {
        spriteRenderer.enabled = visibility;
        spawnIndicator.enabled = !visibility;
    }


    private void Wait()
    {
        attackTimer += Time.deltaTime;
    }

    private void TryAttack()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        if (distanceToPlayer <= playerDetectionRadius)
            Attack();

    }

    private void Attack()
    {
        attackTimer = 0f;

        player.TakeDamage(damage);
    }

    public void TakeDamage(int damage)
    {
        int realDamage = Mathf.Min(damage, health);
        health -= realDamage;


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
