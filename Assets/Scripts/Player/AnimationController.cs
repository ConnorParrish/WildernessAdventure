using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AnimationController : MonoBehaviour
{
    public float SmoothingVelocity;
    public float SmoothingTime;
    public float MaxVelocity;

    private VirtualController virtualController;

    private int movementMagnitudeHash;
    private int isWalkingHash;
    private int isRunningHash;
    private int xVelocityHash;
    private int zVelocityHash;

    
    public void Initialize()
    {
        virtualController = GetComponent<VirtualController>();

        movementMagnitudeHash = Animator.StringToHash("MovementMagnitude");
        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");

        xVelocityHash = Animator.StringToHash("XVelocity");
        zVelocityHash = Animator.StringToHash("ZVelocity");
    }

    public void Locomotion(ref Animator animator)
    {
        bool isWalking = animator.GetBool(isWalkingHash); 
        bool isRunning = animator.GetBool(isRunningHash);

        animator.SetFloat(xVelocityHash, virtualController.CurrentMovement.x, SmoothingTime, Time.deltaTime);
        animator.SetFloat(zVelocityHash, virtualController.CurrentMovement.z, SmoothingTime, Time.deltaTime);

        animator.SetBool(isWalkingHash, virtualController.isMovementPressed);
        animator.SetFloat(movementMagnitudeHash, virtualController.CurrentMovement.magnitude, SmoothingTime, Time.deltaTime);
    }
}
