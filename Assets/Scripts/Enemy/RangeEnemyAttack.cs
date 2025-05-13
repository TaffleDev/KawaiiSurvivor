using UnityEngine;
using UnityEngine.Pool;

public class RangeEnemyAttack : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Transform shootingPoint;
    [SerializeField] private EnemyBullet bulletPrefab;
    private Player player;

    [Header("Settings")]
    [SerializeField] private int damage;
    [SerializeField] private float attackFrequency;

    [Header("Pooling")]
    private ObjectPool<EnemyBullet> bulletPool;

    private float attackDelay;
    private float attackTimer;

    private void Awake()
    {

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bulletPool = new ObjectPool<EnemyBullet>(CreateFunction, ActionOnGet, ActionOnRelease, ActionOnDestroy);

        attackDelay = 1 / attackFrequency;
        attackTimer = attackDelay;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private EnemyBullet CreateFunction()
    {
        EnemyBullet bulletInstance = Instantiate(bulletPrefab, shootingPoint.position, Quaternion.identity);
        bulletInstance.Configure(this);

        return bulletInstance;
    }

    private void ActionOnGet(EnemyBullet enemyBullet)
    {
        enemyBullet.Reload();
        enemyBullet.transform.position = shootingPoint.position;

        enemyBullet.gameObject.SetActive(true);
    }

    private void ActionOnRelease(EnemyBullet enemyBullet)
    {
        enemyBullet.gameObject.SetActive(false);

    }

    private void ActionOnDestroy(EnemyBullet enemyBullet)
    {
        Destroy(enemyBullet.gameObject);
    }

    public void ReleaseBullet(EnemyBullet enemyBullet)
    {
        bulletPool.Release(enemyBullet);
    }

    public void StorePlayer(Player player)
    {
        this.player = player;
    }    

    public void AutoAim()
    {

        ManageShooting();
    }

    private void ManageShooting( )
    {
        attackTimer += Time.deltaTime;

        if (attackTimer >= attackDelay)
        {
            attackTimer = 0;
            Shoot();
        }
    }

    private void Shoot( )
    {
        Vector2 direction = (player.GetCenter() - (Vector2)shootingPoint.position).normalized;
        InstantShoot(direction);
    }

    public void InstantShoot(Vector2 direction)
    {
        EnemyBullet bulletInstance = bulletPool.Get();
        bulletInstance.Shoot(damage, direction);
    }

    
}
