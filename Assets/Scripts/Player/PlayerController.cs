using UnityEngine;

[RequireComponent (typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour, IPlayerStatsDependency
{

    [Header("Elements")]
    Rigidbody2D rb2d;


    [Header("Settings")]
    [SerializeField] float baseMoveSpeed;
    float moveSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }
        
    void FixedUpdate()
    {
        if (!GameManager.instance.IsGameState())
        {
            rb2d.linearVelocity = Vector2.zero;
            return;
        }
        
        rb2d.linearVelocity = InputManager.instance.GetMoveVector() * moveSpeed * Time.deltaTime;

    }

    public void UpdateStats(PlayerStatsManager playerStatsManager)
    {
        float moveSpeedPercent = playerStatsManager.GetStatValue(Stat.MoveSpeed) / 100;
        moveSpeed = baseMoveSpeed * (1 + moveSpeedPercent);
    }
}
