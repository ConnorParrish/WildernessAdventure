using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAimController : MonoBehaviour
{
    public GameObject Crosshair;
    public GameObject AimCamera;
    public GameObject MoveCamera;
    public GameObject InMeshArrow;
    public GameObject ArrowPrefab;
    public GameObject ArrowAttachBone;
    public GameObject Spine;
    public Vector3 SpineOffset;
    public bool IsFullyDrawn = false;
    PlayerController playerController;
    ThirdPersonCameraController cameraController;

    public float aimSwivelMultiplier;

    public Vector3 ArrowSpawnOffset;

    RaycastHit AimTargetRCHit;
    int originalSwivelPower;

    GameObject spawnedArrow;

    private void OnDrawGizmos() {
        Gizmos.color = new Color(0,1,0,.5f);
        Gizmos.DrawCube(transform.position + transform.TransformVector(ArrowSpawnOffset), new Vector3(.1f, .1f, .1f));
    }
    
    void Start() {
    }

    // Start is called before the first frame update
    void Awake()
    {
        playerController = gameObject.GetComponent<PlayerController>();
        cameraController = gameObject.GetComponent<ThirdPersonCameraController>();
        // InMeshArrow.SetActive(false); // disable
    }

    /// TODO: Get animation to match smoothly with spawned arrow
    public void AttachArrowToRightHand() {
        Debug.Log("Attaching arrow to hand");
        InMeshArrow.SetActive(true);
    }

    public void DetatchArrowFromRightHand() {
        Debug.Log("Arrow now balancing");
        InMeshArrow.GetComponent<ArrowController>().BeginBalancing();
    }

    /// TODO: Animation here
    public void DetatchArrow(InputAction.CallbackContext context) {
        if (!playerController.isFireTapped) {
            InMeshArrow.GetComponent<ArrowController>().DisableBalancing();
            InMeshArrow.SetActive(false);
        }
    }

    // Not currently used
    public void DestroyArrow() {
        if (!playerController.isFireTapped) {
            Destroy(spawnedArrow);
        }
    }

    public void FireArrow() {
        Debug.Log("Firing arrrow");
        var arrowOffset = transform.position + transform.TransformVector(ArrowSpawnOffset);
        spawnedArrow = Instantiate(ArrowPrefab, null);
        spawnedArrow.transform.position = InMeshArrow.transform.position;// + arrowOffset;
        // spawnedArrow.transform.rotation = InMeshArrow.transform.rotation;
        
        Vector3 camPosition = cameraController.MainCamera.transform.position;
        Vector3 camDirection = cameraController.MainCamera.transform.forward;

        var ray = new Ray(camPosition, camDirection);
        RaycastHit hit;
        if (Physics.Raycast(ray.origin, camDirection, out hit, 500f)) {
            Debug.DrawLine(ray.origin, hit.point, Color.green);
            spawnedArrow.transform.LookAt(hit.point);
        } else {
            var arrowTarget = transform.TransformPoint(0,0,500);
            spawnedArrow.transform.LookAt(arrowTarget);
        }

        
        spawnedArrow.GetComponent<ArrowController>().BeginFlight(AimTargetRCHit.point);
        InMeshArrow.SetActive(false);
        // Debug.Break();
    }

    // Update is called once per frame
    void Update()
    {
        manageCameraState();
    }

    void manageCameraState() {
        if (playerController.isAimPressed) {
            Crosshair.SetActive(true);
            if (!AimCamera.activeInHierarchy) {
                MoveCamera.SetActive(false);
                AimCamera.SetActive(true);
                var multiplier = aimSwivelMultiplier * ((playerController.ControlScheme.name == "Gamepad") ? .05f : 1f);
                cameraController.CameraSwivelSensitivityModifier = multiplier;
            }
        } else if(!playerController.isAimPressed && !MoveCamera.activeInHierarchy){
            Crosshair.SetActive(false);
            MoveCamera.SetActive(true);
            AimCamera.SetActive(false);
            cameraController.CameraSwivelSensitivityModifier = 1.0f;
        }
    }

    private void LateUpdate() {
        if (playerController.isAimPressed) {
            adjustSpine();
        }
    }

    void adjustSpine() {
        Spine.transform.Rotate(new Vector3(
            SpineOffset.x,
            SpineOffset.y,
            cameraController.FollowTarget.transform.eulerAngles.x + SpineOffset.z
        ));
    }
}
