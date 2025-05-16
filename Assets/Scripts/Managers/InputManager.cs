using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;

    [Header("Elements")]
    [SerializeField] private MobileJoystick playerJoystick;

    [Header("Settings")]
    [SerializeField] private bool forceHandHeld;
    
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        
        
        if (SystemInfo.deviceType == DeviceType.Desktop && !forceHandHeld)
            playerJoystick.gameObject.SetActive(false);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector2 GetMoveVector()
    {
        if (SystemInfo.deviceType == DeviceType.Desktop && !forceHandHeld)
            return GetDeskStopMoveVector();
        else 
            return playerJoystick.GetMoveVector();
        
    }

    private Vector2 GetDeskStopMoveVector()
    {
        return new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }
}
