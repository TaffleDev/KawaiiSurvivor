using System;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class CameraShake : MonoBehaviour
{

    [Header("Settings")]
    [SerializeField] private float shakeMagnitude;
    [SerializeField] private float shakeDuration;


    private void Awake() => RangeWeapon.onBulletShot += Shake;
    private void OnDestroy() => RangeWeapon.onBulletShot -= Shake;
    
    
    private void Shake()
    {
        Vector2 direction = Random.onUnitSphere.With(z: 0).normalized;

        transform.localPosition = Vector3.zero;
        
        LeanTween.cancel(gameObject);
        LeanTween.moveLocal(gameObject, direction * shakeMagnitude, shakeDuration).setEase(LeanTweenType.easeShake);
    }
}
