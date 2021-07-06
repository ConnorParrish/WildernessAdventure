using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using Utilities;

public class ThirdPersonCameraController : MonoBehaviour
{
    public GameObject MainCamera;
    public float CameraSmoothOffsetTime = 0.5f;
    public Vector3 CameraSmoothOffsetVelocity;
    public float CameraSwivelTime = 0.5f;
    public float CameraSwivelSensitivity;
    public float CameraSwivelSensitivityModifier = 1.0f;
    public GameObject FollowTarget;
    VirtualController playerController;
    public CinemachineCameraOffset moveCameraOffset;
    // public CinemachineCameraOffset aimCameraOffset;

    public Vector3 leftOffset = new Vector3(-1.44f, -.4f, 0);
    public Vector3 rightOffset = new Vector3(1.44f, -.4f, 0);
    public Vector3 centeredOffset = new Vector3(0, -.4f, -1f);
    
    void Awake()
    {
        playerController = GetComponent<VirtualController>();
    }

    // Update is called once per frame
    void Update()
    {
        cameraRotation();
        cameraTransformation();
    }
    
    void cameraRotation() {
        // Debug.Log(playerController.CameraMovement);
        var cameraMovement = playerController.CameraMovement;
        var modifiedCameraSwivelSensitivity = CameraSwivelSensitivity * CameraSwivelSensitivityModifier;

        // var targetRotation = Mathf.Atan2(cameraMovement.x,cameraMovement.y) * Mathf.Rad2Deg * modifiedCameraSwivelSensitivity;
        // var xAngle = Mathf.SmoothDampAngle(FollowTarget.transform.eulerAngles.x,targetRotation, ref modifiedCameraSwivelSensitivity, CameraSwivelTime, 10f);
        // var zAngle = Mathf.SmoothDampAngle(FollowTarget.transform.eulerAngles.z,targetRotation, ref modifiedCameraSwivelSensitivity, CameraSwivelTime, 10f);
        // FollowTarget.transform.rotation *= Quaternion.AngleAxis(xAngle, Vector3.up);
        // FollowTarget.transform.rotation *= Quaternion.AngleAxis(-zAngle, Vector3.right);
        FollowTarget.transform.rotation *= Quaternion.AngleAxis(cameraMovement.x * modifiedCameraSwivelSensitivity, Vector3.up);
        FollowTarget.transform.rotation *= Quaternion.AngleAxis(-cameraMovement.y * modifiedCameraSwivelSensitivity, Vector3.right);

        var angles = FollowTarget.transform.localEulerAngles;
        angles.z = 0;

        var angle = FollowTarget.transform.localEulerAngles.x;

        if (angle > 180 && angle < 340) {
            angles.x = 340;
        } else if (angle < 180 && angle > 20) {
            angles.x = 20;
        }

        FollowTarget.transform.localEulerAngles = angles;
    }

    void cameraTransformation() {
        if (playerController.CurrentMovement.x > 0) {
            moveCameraOffset.m_Offset = Vector3.SmoothDamp(moveCameraOffset.m_Offset, rightOffset, ref CameraSmoothOffsetVelocity, CameraSmoothOffsetTime,10f);
        } else if (playerController.CurrentMovement.x < 0) {
            moveCameraOffset.m_Offset = Vector3.SmoothDamp(moveCameraOffset.m_Offset, leftOffset, ref CameraSmoothOffsetVelocity, CameraSmoothOffsetTime,10f);;
        } else if (playerController.CurrentMovement.x == 0) {
            moveCameraOffset.m_Offset = Vector3.SmoothDamp(moveCameraOffset.m_Offset, centeredOffset, ref CameraSmoothOffsetVelocity, CameraSmoothOffsetTime,10f);;
        }
    }
}
