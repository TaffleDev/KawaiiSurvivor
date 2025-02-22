using UnityEngine;
using UnityEngine.Pool;

public class DamageTextManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private DamageText damageTextPrefab;

    [Header("Pooling")]
    private ObjectPool<DamageText> damageTextPool;

    private void Awake()
    {
        Enemy.onDamageTaken += EnemyHitCallBack;
    }

    private void OnDestroy()
    {
        Enemy.onDamageTaken -= EnemyHitCallBack;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        damageTextPool = new ObjectPool<DamageText>(CreateFunction, ActionOnGet, ActionOnRelease, ActionOnDestroy);
    }

    private DamageText CreateFunction()
    {
        return Instantiate(damageTextPrefab, transform);
    }

    private void ActionOnGet(DamageText damageText)
    {
        damageText.gameObject.SetActive(true);
    }

    private void ActionOnRelease(DamageText damageText)
    {
        damageText.gameObject.SetActive(false);

    }

    private void ActionOnDestroy(DamageText damageText)
    {
        Destroy(damageText.gameObject);
    }    


    private void EnemyHitCallBack(int damage, Vector2 enemyPos)
    {
        DamageText damageTextInstance = damageTextPool.Get();

        Vector3 spawnPosition = enemyPos + Vector2.up * 1.5f;
        damageTextInstance.transform.position = spawnPosition;

        damageTextInstance.Animate(damage);

        LeanTween.delayedCall(1, () => damageTextPool.Release(damageTextInstance));
    }
}
