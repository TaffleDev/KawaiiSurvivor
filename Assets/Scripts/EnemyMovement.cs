using System;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Elements")]
    Player player;

    [Header("Spawn Dequence Related")]
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] SpriteRenderer spawnIndicator;
    bool hasSpawned;

    [Header("Settings")]
    [SerializeField] float moveSpeed;
    [SerializeField] float playerDetectionRadius;


    [Header("Effects")]
    [SerializeField] ParticleSystem passAwayParticle;

    [Header("Attack")]
    [SerializeField] int damage;
    [SerializeField] float attackFrequency;
    float attackDelay;
    float attackTimer;

    [Header("DEBUG")]
    [SerializeField] bool showGizmos;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = FindFirstObjectByType<Player>();

        if (player == null)
        {
            Debug.LogWarning("No player found, auto-destroying...");
            Destroy(gameObject);
        }

        // Hide the renderer
        spriteRenderer.enabled = false;
        // Show spawn indicator
        spawnIndicator.enabled = true;

        // Scale up and down the spawn indicator
        Vector3 targetScale = spawnIndicator.transform.localScale * 1.2f;
        LeanTween.scale(spawnIndicator.gameObject, targetScale, .3f)
            .setLoopPingPong(4)
            .setOnComplete(SpawnSequenceCompleted);

        // Prevent following & attacking during the spawn sequence



        //Attack time
        attackDelay = 1f / attackFrequency;
        Debug.Log("Attack Delay : " + attackDelay);
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasSpawned)
            return;

        FollowPlayer();


        if (attackTimer >= attackDelay)
            TryAttack();
        else
            Wait();



    }
    void SpawnSequenceCompleted()
    {
        // Show the enemy after the scale animation

        // Hide the Spawn indicator

        // Show the renderer
        spriteRenderer.enabled = true;
        // Hide spawn indicator
        spawnIndicator.enabled = false;

        hasSpawned = true;
    }

    void FollowPlayer()
    {
        Vector2 direction = (player.transform.position - transform.position).normalized;

        Vector2 targetPosition = (Vector2)transform.position + direction * moveSpeed * Time.deltaTime;

        transform.position = targetPosition;
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
        Debug.Log("Dealing " + damage + " damage to the player");
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
