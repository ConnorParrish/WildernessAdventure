using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonMovement : MonoBehaviour
{
    CharacterController characterController;
    VirtualController virtualController;
    ThirdPersonCameraController cameraController;

    public Transform Cam;
    public float JumpSpeed = 8f;
    public float Speed = 6f;
    public float Gravity = 9.8f;
    public float RunMultiplier = 1.5f;
    public float TurnSmoothTime = .05f;
    public float TurnSmoothVelocity;

    public float TurnSnapTime = .025f;
    public float TurnSnapVelocity;

    float vSpeed = 0f;

    Animator animator;
    Transform followTargetTransform;
    int isWalkingHash;
    int isRunningHash;
    int isAimingHash;

    private void Awake() {
        characterController = GetComponent<CharacterController>();
        virtualController = GetComponent<VirtualController>();
        
        animator = GetComponent<Animator>();
        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
        isAimingHash = Animator.StringToHash("isAiming");

        cameraController = GetComponent<ThirdPersonCameraController>();
        followTargetTransform = cameraController.FollowTarget.transform;
    }

    void Update()
    {
        var currentMovement = virtualController.CurrentMovement;

        if (characterController.isGrounded) {
            vSpeed = -.05f;
            if (virtualController.isJumpPressed) {
                vSpeed = JumpSpeed;
            }
        }
        vSpeed -= Gravity * Time.deltaTime;

        float targetAngle = 0f;
        Vector3 moveDir = new Vector3();
        float angle = 0f;
        
        targetAngle = Mathf.Atan2(currentMovement.x, currentMovement.z) * Mathf.Rad2Deg;
        targetAngle += Cam.eulerAngles.y;

        // Prevent rotating FollowTarget
        // cameraController.DetachFromParent();
        cameraController.FollowTarget.transform.parent = null;
        if (!virtualController.isAimPressed) {
            if (currentMovement.magnitude >= 0.1f) {
                angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref TurnSmoothVelocity, TurnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0);
            }  
        } else {
            angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, followTargetTransform.eulerAngles.y, ref TurnSnapVelocity, TurnSnapTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0);
        }

        // Bring FollowTarget back after rotation
        // cameraController.AttachToParent();
        cameraController.FollowTarget.transform.SetParent(gameObject.transform);

        moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        if (currentMovement.magnitude >= 0.1f || !characterController.isGrounded) {
            var movementSpeed = Speed;
            if (virtualController.isRunPressed) {
                movementSpeed *= RunMultiplier;
            }
            var velocity = moveDir.normalized * movementSpeed * Time.deltaTime;
            velocity.y = vSpeed;
            // characterController.Move(velocity);
        }
    }
}
