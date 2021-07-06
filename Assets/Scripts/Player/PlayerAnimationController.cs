using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : AnimationController
{
    public float AimLayerWeight;
    private int isAimingHash;
    private int isDrawnHash;
    private int hasFiredHash;
    private int mouseYHash;
    private ThirdPersonCameraController cameraController;
    private PlayerAimController aimController;
    private Animator animator;
    private PlayerController playerController;

    void Awake()
    {
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
        cameraController = GetComponent<ThirdPersonCameraController>();
        aimController = GetComponent<PlayerAimController>();
        
        isAimingHash = Animator.StringToHash("isAiming");
        isDrawnHash = Animator.StringToHash("isDrawn");
        hasFiredHash = Animator.StringToHash("hasFired");

        mouseYHash = Animator.StringToHash("MouseY");

        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        bool isAiming = animator.GetBool(isAimingHash);
        bool hasFired = animator.GetBool(hasFiredHash);
        
        if (playerController.isAimPressed) {

            if (!isAiming) {
                animator.SetBool(isAimingHash, true);
                animator.SetLayerWeight(1, 1);
            }
            if (playerController.isFireTapped && !hasFired) {
                animator.SetBool(hasFiredHash, true);
                animator.SetLayerWeight(1,1);
            } else if (!playerController.isFireTapped && hasFired) {
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
        Locomotion(ref animator);
    }

    public void setIsDrawn(int isDrawn) {
        animator.SetBool(isDrawnHash, isDrawn == 1);
        aimController.IsFullyDrawn = (isDrawn == 1);
    }

    public void setHasFired(int hasFired) {
        animator.SetBool(hasFiredHash, hasFiredHash == 1);
    }
}
