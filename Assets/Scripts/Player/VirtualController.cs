using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class VirtualController : MonoBehaviour
{
    // public bool isMoving;
    // public bool isRunning;
    // public bool isAiming;
    // public bool isJumping;

    public Animator Animator;
    public Vector3 CameraMovement;
    public Vector3 CurrentMovement;
    public float FireCooldown;
    public bool FireOnCooldown = false;
    public float MovementMultiplier;
    public bool isMovementPressed;
    public bool isRunPressed;
    public bool isAimPressed;
    public bool isFireTapped;
    public bool isJumpPressed;

    private PlayerInput playerInput;
    private PlayerAimController aimController;

    void Awake() {
        aimController = GetComponent<PlayerAimController>();
        playerInput = new PlayerInput();

        playerInput.Player.Look.started += onCameraInput;
        playerInput.Player.Look.canceled += onCameraInput;
        playerInput.Player.Look.performed += onCameraInput;

        playerInput.Player.Move.started += onMovementInput;
        playerInput.Player.Move.canceled += onMovementInput;
        playerInput.Player.Move.performed += onMovementInput;

        playerInput.Player.Run.started += onRunInput;
        playerInput.Player.Run.canceled += onRunInput;

        // playerInput.Player.Aim.started += aimController.attachArrow;
        playerInput.Player.Aim.started += onAimInput;
        playerInput.Player.Aim.canceled += aimController.DetatchArrow;
        playerInput.Player.Aim.canceled += onAimInput;

        playerInput.Player.Fire.performed += onFireInput;

        playerInput.Player.Jump.started += onJumpInput;
        playerInput.Player.Jump.canceled += onJumpInput;
    }

    void OnEnable() {
        playerInput.Player.Enable();
    }
    public void onCameraInput(InputAction.CallbackContext context) {
        Vector2 cameraMovementInput = context.ReadValue<Vector2>();
        CameraMovement.x = cameraMovementInput.x;
        CameraMovement.y = cameraMovementInput.y;
        CameraMovement = CameraMovement.normalized;
    }

    public void onMovementInput(InputAction.CallbackContext context) {
        Vector2 currentMovementInput = context.ReadValue<Vector2>();
        CurrentMovement.x = currentMovementInput.x;
        CurrentMovement.z = currentMovementInput.y;

        isMovementPressed = (CurrentMovement.x != 0 || CurrentMovement.z != 0);
    }

    public void onRunInput(InputAction.CallbackContext context) {
        isRunPressed = context.ReadValueAsButton();
        CurrentMovement = CurrentMovement * ((isRunPressed) ? MovementMultiplier : 1/MovementMultiplier);
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

    public void onJumpInput(InputAction.CallbackContext context) {
        isJumpPressed = context.ReadValueAsButton();
    }

    // COROUTINES
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
}
