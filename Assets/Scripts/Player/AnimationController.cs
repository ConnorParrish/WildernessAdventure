using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AnimationController : MonoBehaviour
{
    public float SmoothingVelocity;
    public float SmoothingTime;
    public float MaxVelocity;

    private Animator animator;
    private VirtualController virtualController;

    private int movementMagnitudeHash;
    private int isWalkingHash;
    private int isRunningHash;
    private int velocityXHash;
    private int velocityZHash;

    
    public void Initialize()
    {
        animator = GetComponent<Animator>();
        virtualController = GetComponent<VirtualController>();

        movementMagnitudeHash = Animator.StringToHash("MovementMagnitude");
        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");

        velocityXHash = Animator.StringToHash("VelocityX");
        velocityZHash = Animator.StringToHash("VelocityZ");
    }

    private void LateUpdate() {
        
    }

    // Update is called once per frame
    public void Locomotion()
    {
        bool isWalking = animator.GetBool(isWalkingHash); 
        bool isRunning = animator.GetBool(isRunningHash);

        animator.SetFloat(velocityXHash, virtualController.CurrentMovement.x, SmoothingTime, Time.deltaTime);
        animator.SetFloat(velocityZHash, virtualController.CurrentMovement.z, SmoothingTime, Time.deltaTime);

        animator.SetBool(isWalkingHash, virtualController.isMovementPressed);
        animator.SetFloat(movementMagnitudeHash, virtualController.CurrentMovement.magnitude, SmoothingTime, Time.deltaTime);
    }
}
