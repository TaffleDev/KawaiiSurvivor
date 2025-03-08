using UnityEngine;


[RequireComponent (typeof(PlayerHealth))]
public class Player : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private CircleCollider2D playerCollider;
    private PlayerHealth playerHealth;

    private void Awake()
    {
        playerHealth = GetComponent<PlayerHealth>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        playerHealth.TakeDamage(damage);
    }

    public Vector2 GetCenter()
    {
        return (Vector2)transform.position + playerCollider.offset;
    }
}
