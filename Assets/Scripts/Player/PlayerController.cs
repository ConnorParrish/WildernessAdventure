using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.InputSystem.Users;

public class PlayerController : VirtualController
{
    public InputControlScheme ControlScheme;
    public bool isAimPressed;
    public bool isFireTapped;
    public bool isTargetAiTapped;
    public float FireCooldown;
    public bool FireOnCooldown = false;

    private PlayerInput playerInput;

    private PlayerAimController aimController;

    void Awake()
    {
        playerInput = new PlayerInput();
        aimController = GetComponent<PlayerAimController>();

        playerInput.Player.Look.started += onCameraInput;
        playerInput.Player.Look.canceled += onCameraInput;
        playerInput.Player.Look.performed += onCameraInput;

        playerInput.Player.Move.started += onMovementInput;
        playerInput.Player.Move.canceled += onMovementInput;
        playerInput.Player.Move.performed += onMovementInput;

        playerInput.Player.Run.started += onRunInput;
        playerInput.Player.Run.canceled += onRunInput;

        playerInput.Player.Aim.started += onAimInput;
        playerInput.Player.Aim.canceled += aimController.DetatchArrow;
        playerInput.Player.Aim.canceled += onAimInput;

        playerInput.Player.Fire.performed += onFireInput;

        playerInput.Player.TargetAI.started += onTargetAi;
        playerInput.Player.TargetAI.canceled += onTargetAi;
        playerInput.Player.TargetAI.performed += onTargetAi;
    }

    #region Input Handling
    void onCameraInput(InputAction.CallbackContext context) {
        Vector2 cameraMovementInput = context.ReadValue<Vector2>();
        base.onCameraInput(cameraMovementInput);
    }
    
    void onAimInput(InputAction.CallbackContext context) {
        isAimPressed = context.ReadValueAsButton();

        if (context.canceled) {
            aimController.IsFullyDrawn = false;
        }
    }

    void onMovementInput(InputAction.CallbackContext context) {
        Vector2 currentMovementInput = context.ReadValue<Vector2>();
        base.onMovementInput(currentMovementInput);
    }

    void onRunInput(InputAction.CallbackContext context) {
        base.onRunInput(context.ReadValueAsButton());
    }

    void onJumpInput(InputAction.CallbackContext context) {
        base.onJumpInput(context.ReadValueAsButton());
    }

    void onFireInput(InputAction.CallbackContext context) {
        if ((context.interaction is TapInteraction) && !FireOnCooldown && aimController.IsFullyDrawn) {
            isFireTapped = true;
            StartCoroutine(FireCooldownTimer());
        }
    }

    void onTargetAi(InputAction.CallbackContext context) {
        isTargetAiTapped = context.ReadValueAsButton();
    }

    
    #endregion

    #region Coroutines
    private IEnumerator FireCooldownTimer() {
        Debug.Log("Fire cooldown started");
        FireOnCooldown = true;
        yield return new WaitForSeconds(FireCooldown);
        Debug.Log("Fire cooldown ended");
        FireOnCooldown = false;
        isFireTapped = false;
        if (isAimPressed) {
            // animator
        }
    }
    #endregion

    // Doesn't work?
    void OnMouseDown() {
        Debug.Log("Mouse clicked!");
        // Cursor.lockState = CursorLockMode.Confined;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnEnable() {
        playerInput.Player.Enable();
        InputUser.onChange += onInputDeviceChange;
    }

    void onInputDeviceChange(InputUser user, InputUserChange change, InputDevice device) {
        if (change == InputUserChange.ControlSchemeChanged) {
            ControlScheme = user.controlScheme.Value;
        }
    }
}
