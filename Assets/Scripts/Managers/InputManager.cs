using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;

    [Header("Elements")]
    [SerializeField] private MobileJoystick playerJoystick;
    [SerializeField] private InputActionAsset actions;
    
    
    [Header("Settings")]
    [SerializeField] private bool forceHandHeld;

    [Header("Input Actions")]
    private InputAction move;
    private InputAction pauseAction;
    
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        
        
        if (SystemInfo.deviceType == DeviceType.Desktop && !forceHandHeld)
            playerJoystick.gameObject.SetActive(false);
        
        move = actions.FindAction("Move");
        pauseAction = actions.FindAction("Pause");

        pauseAction.performed += PauseActionCallback;

    }

    private void PauseActionCallback(InputAction.CallbackContext obj)
    {
        GameManager.instance.PauseButtonCallBack();
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
        return move.ReadValue<Vector2>();


        // return new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }
}
