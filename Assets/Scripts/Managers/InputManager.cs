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
    private InputAction cancelAction;
    private InputAction lockAction;
    private InputAction navigateScrollAction;

    [Header("Actions")]
    public static Action onCancelAction;
    public static Action onLockAction;
    public static Action<float> onNavigateScrollAction;
    
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
        cancelAction = actions.FindAction("Cancel");
        lockAction = actions.FindAction("Lock");
        navigateScrollAction = actions.FindAction("Navigate Scroll");

        pauseAction.performed += PauseActionCallback;
        cancelAction.performed += CancelActionCallback;
        lockAction.performed += LockActionCallback;
        navigateScrollAction.performed += NavigateScrollActionCallback; 

    }
    private void OnDestroy()
    {
        pauseAction.performed -= PauseActionCallback;
        cancelAction.performed -= CancelActionCallback;
        lockAction.performed -= LockActionCallback;
        navigateScrollAction.performed -= NavigateScrollActionCallback;
    }
    
    

    private void PauseActionCallback(InputAction.CallbackContext obj)
    {
        GameManager.instance.PauseButtonCallBack();
    }
    
    private void CancelActionCallback(InputAction.CallbackContext obj)
    {
        onCancelAction?.Invoke();
    }
    
    private void LockActionCallback(InputAction.CallbackContext obj)
    {
        onLockAction?.Invoke();
    }
    

    private void NavigateScrollActionCallback(InputAction.CallbackContext rightStick)
    {
        onNavigateScrollAction?.Invoke(rightStick.ReadValue<float>());
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
