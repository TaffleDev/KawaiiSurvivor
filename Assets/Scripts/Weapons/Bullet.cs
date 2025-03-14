using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class Bullet : MonoBehaviour
{


    [Header("Elements")]
    private Rigidbody2D rb2d;
    private Collider2D collider;
    private RangeWeapon rangeWeapon;

    [Header("Settings")]
    [SerializeField] private float moveSpeed;
    private int damage;
    [SerializeField] private LayerMask enemyMask;
    private Enemy target;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();

        StartCoroutine(ReleaseCoroutine());

    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator ReleaseCoroutine()
    {
        yield return new WaitForSeconds(1);

        rangeWeapon.ReleaseBullet(this);
    }

    public void Configure(RangeWeapon rangeWeapon)
    {
        this.rangeWeapon = rangeWeapon;
    }

    public void Shoot(int damage, Vector2 direction)
    {
        //Invoke("Release", 1);

        this.damage = damage;

        transform.right = direction;
        rb2d.linearVelocity = direction * moveSpeed;
    }
    public void Reload()
    {
        target = null;

        rb2d.linearVelocity = Vector2.zero;
        collider.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (target != null)
            return;

        if ( IsInLayerMask(collider.gameObject.layer, enemyMask))
        {
            target = collider.GetComponent<Enemy>();

            StopCoroutine(ReleaseCoroutine());

            //CancelInvoke();
            Attack(target);
            Destroy(gameObject);
            //Release();
        }

    }
    /*// * This code is for using Invoke instead. 
    // * If i get problems with Coroutine ill go back to this
    private void Release()
    {
        if (!gameObject.activeSelf)
            return;

        rangeWeapon.ReleaseBullet(this);
    }*/

    private void Attack(Enemy enemy)
    {
        enemy.TakeDamage(damage);
    }

    private bool IsInLayerMask(int layer, LayerMask layerMask)
    {
        return (layerMask.value & (1 << layer)) != 0;
    }
}
