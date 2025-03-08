using UnityEngine;
using TMPro;
using System;

[RequireComponent (typeof(EnemyMovement))]
public class MeleeEnemy : Enemy
{       

    [Header("Attack")]
    [SerializeField] private int damage;
    [SerializeField] private float attackFrequency;
    
    private float attackDelay;
    private float attackTimer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        player = FindFirstObjectByType<Player>();
                
        //Attack time
        attackDelay = 1f / attackFrequency;
    }

    // Update is called once per frame
    void Update()
    {
        if (!CanAttack())
            return;

        if (attackTimer >= attackDelay)
            TryAttack();
        else
            Wait();

        enemyMovement.FollowPlayer();
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
}
