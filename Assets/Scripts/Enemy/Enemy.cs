using System;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [Header("Components")]
    protected EnemyMovement enemyMovement;

    [Header("Health")]
    [SerializeField] protected int maxHealth;
    protected int health;

    [Header("Elements")]
    protected Player player;

    [Header("Spawn Dequence Related")]
    [SerializeField] protected SpriteRenderer spriteRenderer;
    [SerializeField] protected SpriteRenderer spawnIndicator;
    [SerializeField] protected Collider2D spriteCollider;
    protected bool hasSpawned;

    [Header("Effects")]
    [SerializeField] protected ParticleSystem passAwayParticle;

    [Header("Attack")]
    [SerializeField] protected float playerDetectionRadius;

    [Header("Actions")]
    // Damage - position - isCritical
    public static Action<int, Vector2, bool> onDamageTaken;
    // Position
    public static Action<Vector2> onPassedAway;
    
    public static Action<Vector2> onBossPassedAway;
    protected Action onSpawnSequenceCompleted;

    [Header("DEBUG")]
    [SerializeField] protected bool showGizmos;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
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
    }

    // Update is called once per frame
    protected bool CanAttack()
    {
        return spriteRenderer.enabled;
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

        if (enemyMovement != null)
            enemyMovement.StorePlayer(player);
        
        onSpawnSequenceCompleted?.Invoke();
    }

    private void SetRenderersVisibility(bool visibility)
    {
        spriteRenderer.enabled = visibility;
        spawnIndicator.enabled = !visibility;
    }

    public void TakeDamage(int damage, bool isCriticalHit)
    {
        int realDamage = Mathf.Min(damage, health);
        health -= realDamage;

        onDamageTaken?.Invoke(damage, transform.position, isCriticalHit);

        if (health <= 0)
            PassAway();
    }

    public virtual void PassAway()
    {
        onPassedAway?.Invoke(transform.position);

        PassAwayAfterWave();
    }

    public void PassAwayAfterWave()
    {
        passAwayParticle.transform.SetParent(null);
        passAwayParticle.Play();

        Destroy(gameObject);
    }
    
    public Vector2 GetCenter()
    {
        return (Vector2)transform.position + spriteCollider.offset;
    }
    
    private void OnDrawGizmos()
    {
        if (!showGizmos)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, playerDetectionRadius);

    }
}
