using System.Collections;
using UnityEngine;

[RequireComponent (typeof(Rigidbody2D), typeof(Collider2D))]
public class EnemyBullet : MonoBehaviour
{
    [Header("Elements")]
    private Rigidbody2D rb2d;
    private Collider2D collider;
    private RangeEnemyAttack rangeEnemyAttack;

    [Header("Settings")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float angularSpeed; 
    private int damage;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();


        ////Despawns bullet after 5 seconds
        LeanTween.delayedCall(gameObject, 5, () => rangeEnemyAttack.ReleaseBullet(this));
        // StartCoroutine(ReleaseCoroutine());
    }

    // Despawns bullet the same way as Leantween, just using Coroutine
    // IEnumerator ReleaseCoroutine()
    // {
    //     yield return new WaitForSeconds(5);
    //
    //     rangeEnemyAttack.ReleaseBullet(this);
    // }

    public void Configure(RangeEnemyAttack rangeEnemyAttack)
    {
        this.rangeEnemyAttack = rangeEnemyAttack;
    }

    public void Shoot(int damage, Vector2 direction)
    {
        this.damage = damage;

        // The left most star is changing a rotation direction and this fixes that.
        if (Mathf.Abs(direction.x + 1) < 0.01f)
            direction.y += .01f;

        transform.right = direction;
        rb2d.linearVelocity = direction * moveSpeed;
        
        rb2d.angularVelocity = angularSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.TryGetComponent(out Player player))
        {
            LeanTween.cancel(gameObject);
            // StopCoroutine(ReleaseCoroutine());


            player.TakeDamage(damage);
            this.collider.enabled = false;

            rangeEnemyAttack.ReleaseBullet(this);
        }
    }

    public void Reload()
    {
        rb2d.linearVelocity = Vector2.zero;
        rb2d.angularVelocity = 0;
        
        collider.enabled = true;
        
        LeanTween.cancel(gameObject);
        LeanTween.delayedCall(gameObject, 5, () => rangeEnemyAttack.ReleaseBullet(this));

        
    }
}
