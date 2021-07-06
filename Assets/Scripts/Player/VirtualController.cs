using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class VirtualController : MonoBehaviour
{
    public Vector3 CameraMovement;
    public Vector3 CurrentMovement;

    public float MovementMultiplier;
    public bool isMovementPressed;
    public bool isRunPressed;
    public bool isJumpPressed;

    public void onCameraInput(Vector2 cameraMovementInput) {
        CameraMovement.x = cameraMovementInput.x;
        CameraMovement.y = cameraMovementInput.y;
        CameraMovement = CameraMovement.normalized;
    }
    public void onMovementInput(Vector2 currentMovementInput) {
        CurrentMovement.x = currentMovementInput.x;
        CurrentMovement.z = currentMovementInput.y;

        isMovementPressed = (CurrentMovement.x != 0 || CurrentMovement.z != 0);
    }

    public void onRunInput(bool runInput) {
        isRunPressed = runInput;
        CurrentMovement = CurrentMovement * ((isRunPressed) ? MovementMultiplier : 1/MovementMultiplier);
    }
    
    public void onJumpInput(bool jumpInput) {
        isJumpPressed = jumpInput;
    }
}
