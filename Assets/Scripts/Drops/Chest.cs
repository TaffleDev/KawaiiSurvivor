using UnityEditor.Experimental.GraphView;
using UnityEngine;
using System;

public class Chest : MonoBehaviour, ICollectable
{

    [Header("Actions")]
    public static Action onCollected;
    public void Collect(Player playet)
    {
        onCollected?.Invoke();
        Destroy(gameObject);
    }

    
}
