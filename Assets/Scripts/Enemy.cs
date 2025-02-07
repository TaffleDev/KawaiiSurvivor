using UnityEngine;

[RequireComponent (typeof(EnemyMovement))]
public class Enemy : MonoBehaviour
{
    [Header("Components")]
    EnemyMovement enemyMovement;

    [Header("Elements")]
    Player player;

    [Header("Spawn Dequence Related")]
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] SpriteRenderer spawnIndicator;
    bool hasSpawned;

    [Header("Effects")]
    [SerializeField] ParticleSystem passAwayParticle;


    [Header("Attack")]
    [SerializeField] int damage;
    [SerializeField] float attackFrequency;
    [SerializeField] float playerDetectionRadius;

    float attackDelay;
    float attackTimer;

    [Header("DEBUG")]
    [SerializeField] bool showGizmos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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

    void StartSpawnSequence()
    {
        spriteRenderer.enabled = false;
        spawnIndicator.enabled = true;

        Vector3 targetScale = spawnIndicator.transform.localScale * 1.2f;
        LeanTween.scale(spawnIndicator.gameObject, targetScale, .3f)
            .setLoopPingPong(4)
            .setOnComplete(SpawnSequenceCompleted);

    }

    void SpawnSequenceCompleted()
    {        
        spriteRenderer.enabled = true;
        spawnIndicator.enabled = false;
        hasSpawned = true;

        enemyMovement.StorePlayer(player);
    }

    void SetRenderersVisibility(bool visibility)
    {
        spriteRenderer.enabled = visibility;
        spawnIndicator.enabled = !visibility;
    }
        

    void Wait()
    {
        attackTimer += Time.deltaTime;
    }

    private void TryAttack()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        if (distanceToPlayer <= playerDetectionRadius)
            Attack();

    }

    void Attack()
    {
        attackTimer = 0f;
    }

    void PassAway()
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
