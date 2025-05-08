using System;
using UnityEngine;

public class HapticFeedback : MonoBehaviour
{
    private void Awake()
    {
        RangeWeapon.onBulletShot += Vibrate;
    }

    private void OnDestroy()
    {
        RangeWeapon.onBulletShot -= Vibrate;
    }

    private void Vibrate()
    {
        CandyCoded.HapticFeedback.HapticFeedback.LightFeedback();
    }
}
