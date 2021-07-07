using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentCommander : MonoBehaviour
{
    public WolfController wolfController;
    public Material TargettedMaterial; // Testing
    private PlayerController playerController;
    private ThirdPersonCameraController cameraController;
    private Camera mainCamera;
    private void Awake() {
        playerController = GetComponent<PlayerController>();
        cameraController = GetComponent<ThirdPersonCameraController>();
        mainCamera = Camera.main;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (playerController.isTargetAiTapped) {
            var ray = Camera.main.ViewportPointToRay(new Vector3(0.5f,0.5f,0f));
            RaycastHit hit;
            if(Physics.Raycast(ray.origin, mainCamera.transform.forward, out hit)) {
                wolfController.NavTarget = hit.transform.gameObject;
                var renderer = hit.transform.GetComponent<Renderer>();
                renderer.material = TargettedMaterial;
            }
        }
    }
}
