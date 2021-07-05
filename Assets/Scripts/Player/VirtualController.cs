using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class VirtualController : MonoBehaviour
{
    public Vector3 CurrentMovement;
    public float MovementMultiplier;
    public bool isMovementPressed;
    public bool isRunPressed;
    public bool isJumpPressed;

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
    
    public void onJumpInput(InputAction.CallbackContext context) {
        isJumpPressed = context.ReadValueAsButton();
    }
}
