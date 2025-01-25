using UnityEngine;

[RequireComponent (typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{

    [Header("Elements")]
    [SerializeField] MobileJoystick playerJoystick;
    Rigidbody2D rb2d;


    [Header("Settings")]
    [SerializeField] float moveSpeed;
    



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.linearVelocity = Vector2.right;
    }
        
    void FixedUpdate()
    {
        rb2d.linearVelocity = playerJoystick.GetMoveVector() * moveSpeed * Time.deltaTime;

    }
}
