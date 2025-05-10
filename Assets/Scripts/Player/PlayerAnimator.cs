using System;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class PlayerAnimator : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Animator animator;
    private Rigidbody2D rb;

    private void Awake() => rb = GetComponent<Rigidbody2D>();


    private void FixedUpdate()
    {
        if (rb.linearVelocity.magnitude < 0.0001)
            animator.Play("Idle");
        else
            animator.Play("Move");
    }
}
