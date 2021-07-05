using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;


public class PlayerController : VirtualController
{
    public Vector3 CameraMovement;
    public bool isAimPressed;
    public bool isFireTapped;
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

        playerInput.Player.Move.started += base.onMovementInput;
        playerInput.Player.Move.canceled += base.onMovementInput;
        playerInput.Player.Move.performed += base.onMovementInput;

        playerInput.Player.Run.started += base.onRunInput;
        playerInput.Player.Run.canceled += base.onRunInput;

        playerInput.Player.Aim.started += onAimInput;
        playerInput.Player.Aim.canceled += aimController.DetatchArrow;
        playerInput.Player.Aim.canceled += onAimInput;

        playerInput.Player.Fire.performed += onFireInput;


    }

    #region Input Handling
    public void onCameraInput(InputAction.CallbackContext context) {
        Vector2 cameraMovementInput = context.ReadValue<Vector2>();
        CameraMovement.x = cameraMovementInput.x;
        CameraMovement.y = cameraMovementInput.y;
        CameraMovement = CameraMovement.normalized;
    }
    
    public void onAimInput(InputAction.CallbackContext context) {
        isAimPressed = context.ReadValueAsButton();

        if (context.canceled) {
            aimController.IsFullyDrawn = false;
        }
    }

    public void onFireInput(InputAction.CallbackContext context) {
        // isFirePressed = context.ReadValueAsButton();
        if ((context.interaction is TapInteraction) && !FireOnCooldown && aimController.IsFullyDrawn) {
            // aimController.FireArrow();
            isFireTapped = true;
            StartCoroutine(FireCooldownTimer());
        }
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
    private void OnMouseDown() {
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void OnEnable() {
        playerInput.Player.Enable();
    }
}
