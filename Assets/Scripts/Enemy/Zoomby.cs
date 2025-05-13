using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Random = UnityEngine.Random;


[RequireComponent(typeof(RangeEnemyAttack))]
public class Zoomby : Enemy
{
    [Header("Elements")]
    [SerializeField] private Slider healthBar;
    [SerializeField] private  TextMeshProUGUI healthText;
    [SerializeField] private Animator animator;
    
    enum State { None, Idle, Moving, Attacking }

    [Header("State Machine")]
    private State state;
    private float timer;

    [Header("Idle State")]
    private float idleDuration;
    [SerializeField] private float maxIdleDuration;

    [Header("Moving State")]
    [SerializeField] private float moveSpeed;
    private Vector2 targetPosition;

    [Header("Attack State")]
    private int attackCounter;
    private RangeEnemyAttack attack;
    
    
    private void Awake()
    {
        state = State.None;
        
        healthBar.gameObject.SetActive(false);
        
        onSpawnSequenceCompleted += SpawnSequenceCompletedCallback;
        onDamageTaken += DamageTakenCallback;
    }

    private void OnDestroy()
    {
        onSpawnSequenceCompleted -= SpawnSequenceCompletedCallback;
        onDamageTaken -= DamageTakenCallback;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        attack = GetComponent<RangeEnemyAttack>();

    }

    // Update is called once per frame
    void Update()
    {
        ManageStates();
    }

    private void ManageStates()
    {
        switch (state)
        {
            case State.Idle:
                ManageIdleState();
                break;
            
            case State.Moving:
                ManageMovingState();
                break;
            
            case State.Attacking:
                ManageAttackState();
                break;
            
            default:
                break;
        }
    }

    private void SetIdleState()
    {
        state = State.Idle;

        idleDuration = Random.Range(1f, maxIdleDuration);
        
        animator.Play("Idle");
    }
    
    private void ManageIdleState()
    {
        timer += Time.deltaTime;

        if (timer >= idleDuration)
        {
            timer = 0;
            StartMovingState();
        }
    }
    
    private void StartMovingState()
    {
        state = State.Moving;

        targetPosition = GetRandomPosition();
        
        animator.Play("Move");
    }
    
    private void ManageMovingState()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, targetPosition) < .01f)
            StartAttackingState();
    }

    private void StartAttackingState()
    {
        // Debug.Log("Attacking state");
        state = State.Attacking;
        attackCounter = 0;
        
        animator.Play("Attack");

    }
    
    private void ManageAttackState()
    {
        
    }

    
    
    private void Attack()
    {
        Vector2 direction = Quaternion.Euler(0, 0, -45 * attackCounter) * Vector2.right;
        attack.InstantShoot(direction);
        attackCounter++;
        

    }

    public override void PassAway()
    {
        onBossPassedAway?.Invoke(transform.position);
        PassAwayAfterWave();   
    }

    private Vector2 GetRandomPosition()
    {
        Vector2 targetPosition = Vector2.zero;

        targetPosition.x = Random.Range(-14f, 14);
        targetPosition.y = Random.Range(-6f, 6);

        return targetPosition;
    }
    
    
    private void SpawnSequenceCompletedCallback()
    {
        healthBar.gameObject.SetActive(true);
        UpdateHealthBar();

        SetIdleState();
    }

    private void UpdateHealthBar()
    {
        healthBar.value = (float)health / maxHealth;
        healthText.text = $"{health} / {maxHealth}";
        
    }

    private void DamageTakenCallback(int damage, Vector2 position, bool isCritical)
    {
        UpdateHealthBar();
    }
}
