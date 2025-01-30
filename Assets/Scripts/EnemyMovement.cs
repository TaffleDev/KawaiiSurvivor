using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Elements")]
    Player player;

    [Header("Settings")]
    [SerializeField] float moveSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = FindFirstObjectByType<Player>();

        if (player == null)
        {
            Debug.LogWarning("No player found, auto-destroying...");
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 direction = (player.transform.position - transform.position).normalized;

        Vector2 targetPosition = (Vector2)transform.position + direction * moveSpeed * Time.deltaTime;

        transform.position = targetPosition;

    }
}
