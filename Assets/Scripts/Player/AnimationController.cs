using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public float SmoothingVelocity;
    public float SmoothingTime;
    public float MaxVelocity;
    public float AimLayerWeight;

    private Animator animator;
    private VirtualController virtualController;
    private ThirdPersonCameraController cameraController;
    private PlayerAimController aimController;

    private int movementMagnitudeHash;
    private int isWalkingHash;
    private int isRunningHash;
    private int isAimingHash;
    private int isDrawnHash;
    private int hasFiredHash;
    private int velocityXHash;
    private int velocityZHash;
    private int mouseYHash;

    
    void Awake()
    {
        animator = GetComponent<Animator>();
        virtualController = GetComponent<VirtualController>();
        cameraController = GetComponent<ThirdPersonCameraController>();
        aimController = GetComponent<PlayerAimController>();

        movementMagnitudeHash = Animator.StringToHash("MovementMagnitude");
        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
        isAimingHash = Animator.StringToHash("isAiming");
        isDrawnHash = Animator.StringToHash("isDrawn");
        hasFiredHash = Animator.StringToHash("hasFired");

        velocityXHash = Animator.StringToHash("VelocityX");
        velocityZHash = Animator.StringToHash("VelocityZ");

        mouseYHash = Animator.StringToHash("MouseY");
    }

    private void LateUpdate() {
        
    }

    // Update is called once per frame
    void Update()
    {
        bool isAiming = animator.GetBool(isAimingHash);
        bool isWalking = animator.GetBool(isWalkingHash); 
        bool isRunning = animator.GetBool(isRunningHash);
        bool hasFired = animator.GetBool(hasFiredHash);

        if (virtualController.isAimPressed) {
            animator.SetFloat(velocityXHash, virtualController.CurrentMovement.x, SmoothingTime, Time.deltaTime);
            animator.SetFloat(velocityZHash, virtualController.CurrentMovement.z, SmoothingTime, Time.deltaTime);

            if (!isAiming) {
                animator.SetBool(isAimingHash, true);
                animator.SetLayerWeight(1, 1);
            }
            if (virtualController.isFireTapped && !hasFired) {
                animator.SetBool(hasFiredHash, true);
                animator.SetLayerWeight(1,1);
            } else if (!virtualController.isFireTapped && hasFired) {
                animator.SetBool(hasFiredHash, false);
            }
        } else {
            if (isAiming) {
                animator.SetBool(isAimingHash, false);
            }
            if (animator.GetBool(isDrawnHash)) {
                animator.SetBool(isDrawnHash, false);
            }
            animator.SetLayerWeight(1, 0);

        }

        animator.SetBool(isWalkingHash, virtualController.isMovementPressed);
        animator.SetFloat(movementMagnitudeHash, virtualController.CurrentMovement.magnitude, SmoothingTime, Time.deltaTime);
    }

    public void setIsDrawn(int isDrawn) {
        animator.SetBool(isDrawnHash, isDrawn == 1);
        aimController.IsFullyDrawn = (isDrawn == 1);
        // animator.SetLayerWeight(1, 0);
    }

    public void setHasFired(int hasFired) {
        animator.SetBool(hasFiredHash, hasFiredHash == 1);
    }
}
